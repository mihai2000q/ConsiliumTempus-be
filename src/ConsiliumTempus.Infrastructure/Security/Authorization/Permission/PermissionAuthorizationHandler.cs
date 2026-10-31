using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Infrastructure.Security.Authorization.NotFound;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using ConsiliumTempus.Infrastructure.Security.Authorization.Targets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ConsiliumTempus.Infrastructure.Security.Authorization.Permission;

internal sealed class PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var subUserId = context.User.Claims
            .SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
        if (!Guid.TryParse(subUserId, out var guidUserId)) return;

        using var scope = serviceScopeFactory.CreateScope();
        var permissionProvider = scope.ServiceProvider.GetRequiredService<IPermissionProvider>();
        var httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
        var workspaceProvider = scope.ServiceProvider.GetRequiredService<IWorkspaceProvider>();

        var request = httpContextAccessor.HttpContext?.Request;
        if (request is null) return;

        var (workspaceId, idType, hasValidId) =
            await GetWorkspaceId(request, workspaceProvider);
        if (!hasValidId)
        {
            context.Succeed(requirement); // let the system return the validation error
        }
        else if (workspaceId is null)
        {
            context.Fail(new NotFoundAuthorizationFailureReason(this, idType.NotFoundError()));
        }
        else
        {
            var userId = UserId.Create(guidUserId);
            var permissions = await permissionProvider.GetPermissions(userId, workspaceId);
            if (permissions is null) // not a collaborator, so the user must not know that the resource exists
                context.Fail(new NotFoundAuthorizationFailureReason(this, idType.NotFoundError()));
            else if (permissions.Contains(requirement.Permission.ToString())) context.Succeed(requirement);
        }
    }

    /// <summary>
    /// Get Workspace id from query, route or body of the request or related entity
    /// <returns>
    /// the workspace (null when it was not found), the type of the id used to find it,
    /// and whether there is a valid id at all (when there is not, it is a validation error)
    /// </returns>
    /// </summary>
    private static async Task<(WorkspaceId?, TargetKind, bool)> GetWorkspaceId(
        HttpRequest request,
        IWorkspaceProvider workspaceProvider)
    {
        var target = AuthorizationTargets.For(request);

        if (!Guid.TryParse(await target.ReadAsync(request), out var guidId) || guidId == Guid.Empty)
            return (null, target.Kind, false);

        var workspace = target.Kind switch
        {
            TargetKind.CustomFieldSetup => await workspaceProvider.GetByCustomFieldSetup(CustomFieldSetupId.Create(guidId)),
            TargetKind.Project => await workspaceProvider.GetByProject(ProjectId.Create(guidId)),
            TargetKind.ProjectSprint => await workspaceProvider.GetByProjectSprint(ProjectSprintId.Create(guidId)),
            TargetKind.ProjectStage => await workspaceProvider.GetByProjectStage(ProjectStageId.Create(guidId)),
            TargetKind.ProjectTask => await workspaceProvider.GetByProjectTask(ProjectTaskId.Create(guidId)),
            TargetKind.Workspace => await workspaceProvider.Get(WorkspaceId.Create(guidId)),
            _ => throw new ArgumentOutOfRangeException(nameof(target), target.Kind, null)
        };

        return (workspace?.Id, target.Kind, true);
    }
}
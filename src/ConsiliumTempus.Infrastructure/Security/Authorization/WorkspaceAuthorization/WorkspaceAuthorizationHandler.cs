using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Infrastructure.Security.Authorization.NotFound;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using ConsiliumTempus.Infrastructure.Security.Authorization.Targets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ConsiliumTempus.Infrastructure.Security.Authorization.WorkspaceAuthorization;

internal sealed class WorkspaceAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    : AuthorizationHandler<WorkspaceAuthorizationRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        WorkspaceAuthorizationRequirement requirement)
    {
        var subUserId = context.User.Claims
            .SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
        if (!Guid.TryParse(subUserId, out var guidUserId)) return;

        using var scope = serviceScopeFactory.CreateScope();
        var httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
        var workspaceProvider = scope.ServiceProvider.GetRequiredService<IWorkspaceProvider>();

        var request = httpContextAccessor.HttpContext?.Request;
        if (request is null) return;

        var (workspace, idType, hasValidId) = await GetWorkspace(request, workspaceProvider);
        if (!hasValidId)
        {
            context.Succeed(requirement); // let the system return the validation error
        }
        else if (workspace is null)
        {
            context.Fail(new NotFoundAuthorizationFailureReason(this, idType.NotFoundError()));
        }
        else
        {
            var userId = UserId.Create(guidUserId);
            if (IsAuthorized(requirement.AuthorizationLevel, workspace, userId))
                context.Succeed(requirement);
            else if (!IsCollaborator(workspace, userId)) // the user must not know that the resource exists
                context.Fail(new NotFoundAuthorizationFailureReason(this, idType.NotFoundError()));
        }
    }

    private static async Task<(WorkspaceAggregate?, TargetKind, bool)> GetWorkspace(
        HttpRequest request,
        IWorkspaceProvider workspaceProvider)
    {
        var target = AuthorizationTargets.For(request);

        if (!Guid.TryParse(await target.ReadAsync(request), out var guidId) || guidId == Guid.Empty)
            return (null, target.Kind, false);

        var workspace = target.Kind switch
        {
            TargetKind.Workspace => await workspaceProvider.GetWithMemberships(WorkspaceId.Create(guidId)),
            TargetKind.Project => await workspaceProvider.GetByProjectWithMemberships(ProjectId.Create(guidId)),
            _ => throw new ArgumentOutOfRangeException(nameof(target), target.Kind, null)
        };
        return (workspace, target.Kind, true);
    }

    private static bool IsCollaborator(WorkspaceAggregate workspace, UserId userId)
    {
        return workspace.Memberships.Any(m => m.User.Id == userId);
    }

    private static bool IsAuthorized(
        WorkspaceAuthorizationLevel authorizationLevel,
        WorkspaceAggregate workspace,
        UserId userId)
    {
        return authorizationLevel switch
        {
            WorkspaceAuthorizationLevel.IsCollaborator => IsCollaborator(workspace, userId),
            WorkspaceAuthorizationLevel.IsWorkspaceOwner => workspace.Owner.Id == userId,
            _ => throw new ArgumentOutOfRangeException(nameof(authorizationLevel))
        };
    }
}
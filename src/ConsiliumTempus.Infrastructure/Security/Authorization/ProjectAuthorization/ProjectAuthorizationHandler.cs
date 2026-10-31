using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Infrastructure.Security.Authorization.NotFound;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using ConsiliumTempus.Infrastructure.Security.Authorization.Targets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ConsiliumTempus.Infrastructure.Security.Authorization.ProjectAuthorization;

internal sealed class ProjectAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    : AuthorizationHandler<ProjectAuthorizationRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ProjectAuthorizationRequirement requirement)
    {
        var subUserId = context.User.Claims
            .SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
        if (!Guid.TryParse(subUserId, out var guidUserId)) return;

        using var scope = serviceScopeFactory.CreateScope();
        var httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
        var projectProvider = scope.ServiceProvider.GetRequiredService<IProjectProvider>();

        var request = httpContextAccessor.HttpContext?.Request;
        if (request is null) return;

        var (project, idType, hasValidId) = await GetProject(request, projectProvider);
        if (!hasValidId)
        {
            context.Succeed(requirement); // let the system return the validation error
        }
        else if (project is null && idType == TargetKind.CustomFieldSetup)
        {
            // global custom field setups do not belong to a project, and the ones that do not exist
            // are already reported by the permission authorization, which finds the workspace of the setup
            context.Succeed(requirement);
        }
        else if (project is null)
        {
            context.Fail(new NotFoundAuthorizationFailureReason(this, idType.NotFoundError()));
        }
        else
        {
            var userId = UserId.Create(guidUserId);
            if (IsAuthorized(requirement.AuthorizationLevel, project, userId))
                context.Succeed(requirement);
            else if (!IsAllowed(project, userId)) // the user must not know that the resource exists
                context.Fail(new NotFoundAuthorizationFailureReason(this, idType.NotFoundError()));
        }
    }

    private static async Task<(ProjectAggregate?, TargetKind, bool)> GetProject(
        HttpRequest request,
        IProjectProvider projectProvider)
    {
        var target = AuthorizationTargets.ForProject(request);

        if (!Guid.TryParse(await target.ReadAsync(request), out var guidId) || guidId == Guid.Empty)
            return (null, target.Kind, false);

        var project = target.Kind switch
        {
            TargetKind.CustomFieldSetup => await projectProvider.GetByCustomFieldSetup(CustomFieldSetupId.Create(guidId)),
            TargetKind.Project => await projectProvider.Get(ProjectId.Create(guidId)),
            TargetKind.ProjectSprint => await projectProvider.GetByProjectSprint(ProjectSprintId.Create(guidId)),
            TargetKind.ProjectStage => await projectProvider.GetByProjectStage(ProjectStageId.Create(guidId)),
            TargetKind.ProjectTask => await projectProvider.GetByProjectTask(ProjectTaskId.Create(guidId)),
            _ => throw new ArgumentOutOfRangeException(nameof(target), target.Kind, null)
        };
        return (project, target.Kind, true);
    }

    private static bool IsAllowed(ProjectAggregate project, UserId userId)
    {
        return !project.IsPrivate.Value || project.AllowedMembers.Any(u => u.Id == userId);
    }

    private static bool IsAuthorized(
        ProjectAuthorizationLevel authorizationLevel,
        ProjectAggregate project,
        UserId userId)
    {
        return authorizationLevel switch
        {
            ProjectAuthorizationLevel.IsAllowed => IsAllowed(project, userId),
            ProjectAuthorizationLevel.IsProjectOwner => project.Owner.Id == userId,
            _ => throw new ArgumentOutOfRangeException(nameof(authorizationLevel))
        };
    }
}
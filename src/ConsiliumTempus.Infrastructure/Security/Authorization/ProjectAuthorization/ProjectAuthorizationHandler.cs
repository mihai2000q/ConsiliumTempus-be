using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Infrastructure.Extensions;
using ConsiliumTempus.Infrastructure.Security.Authorization.Http;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ConsiliumTempus.Infrastructure.Security.Authorization.ProjectAuthorization;

public sealed class ProjectAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
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

        var project = await GetProject(request, projectProvider);
        if (project is null)
        {
            context.Succeed(requirement); // let the system return the not found or validation error 
        }
        else
        {
            var userId = UserId.Create(guidUserId);
            if (IsAuthorized(requirement.AuthorizationLevel, project, userId)) context.Succeed(requirement);
        }
    }

    private static async Task<ProjectAggregate?> GetProject(HttpRequest request, IProjectProvider projectProvider)
    {
        var (stringId, idType) = await GetStringId(request);
        if (!Guid.TryParse(stringId, out var guidId)) return null;
        return idType switch
        {
            StringIdType.Project => await projectProvider.Get(ProjectId.Create(guidId)),
            StringIdType.ProjectSprint => await projectProvider.GetByProjectSprint(ProjectSprintId.Create(guidId)),
            StringIdType.ProjectStage => await projectProvider.GetByProjectStage(ProjectStageId.Create(guidId)),
            StringIdType.ProjectTask => await projectProvider.GetByProjectTask(ProjectTaskId.Create(guidId)),
            StringIdType.Empty => null,
            _ => throw new ArgumentOutOfRangeException(nameof(request))
        };
    }

    private static async Task<(string?, StringIdType)> GetStringId(HttpRequest request)
    {
        return request.RouteValues["controller"] switch
        {
            "Project" => request.RouteValues["action"] switch
            {
                "Get" or
                "GetOverview" or
                "GetStatuses" or
                "Delete" or
                "RemoveStatus" => (HttpRequestReader.GetStringIdFromRoute(request), StringIdType.Project),

                "AddAllowedMember" or
                "AddStatus" or
                "Update" or
                "UpdateFavorites" or
                "UpdateOverview" or
                "UpdateStatus" => (await HttpRequestReader.GetStringIdFromBody(request), StringIdType.Project),

                _ => (null, StringIdType.Empty)
            },
            "ProjectSprint" => request.RouteValues["action"] switch
            {
                "Get" or
                "GetStages" or
                "Delete" or
                "RemoveStage" => (HttpRequestReader.GetStringIdFromRoute(request), StringIdType.ProjectSprint),

                "GetCollection" => (
                    HttpRequestReader.GetStringIdFromQuery(request, typeof(ProjectAggregate).ToCamelId()), 
                    StringIdType.Project),

                "Create" => (
                    await HttpRequestReader.GetStringIdFromBody(request, typeof(ProjectAggregate).ToCamelId()), 
                    StringIdType.Project),

                "AddStage" or
                "Update" or
                "MoveStage" or
                "UpdateStage" => (await HttpRequestReader.GetStringIdFromBody(request), StringIdType.ProjectSprint),

                _ => (null, StringIdType.Empty)
            },
            "ProjectTask" => request.RouteValues["action"] switch
            {
                "Get" or
                "Delete" => (HttpRequestReader.GetStringIdFromRoute(request), StringIdType.ProjectTask),

                "GetCollection" => (
                    HttpRequestReader.GetStringIdFromQuery(request, typeof(ProjectStage).ToCamelId()), 
                    StringIdType.ProjectStage),

                "Create" => (
                    await HttpRequestReader.GetStringIdFromBody(request, typeof(ProjectStage).ToCamelId()), 
                    StringIdType.ProjectStage),

                "Move" or
                "Update" or
                "UpdateIsCompleted" or
                "UpdateOverview" => (await HttpRequestReader.GetStringIdFromBody(request), StringIdType.ProjectTask),

                _ => (null, StringIdType.Empty)
            },
            _ => (null, StringIdType.Empty)
        };
    }

    private static bool IsAuthorized(
        ProjectAuthorizationLevel authorizationLevel,
        ProjectAggregate project,
        UserId userId)
    {
        return authorizationLevel switch
        {
            ProjectAuthorizationLevel.IsAllowed => !project.IsPrivate.Value ||
                                                   project.AllowedMembers.Any(u => u.Id == userId),
            ProjectAuthorizationLevel.IsProjectOwner => project.Owner.Id == userId,
            _ => throw new ArgumentOutOfRangeException(nameof(authorizationLevel))
        };
    }

    private enum StringIdType
    {
        Empty,
        Project,
        ProjectSprint,
        ProjectStage,
        ProjectTask
    }
}
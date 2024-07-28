using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.ValueObjects;
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
        if (subUserId is null || !Guid.TryParse(subUserId, out var guidUserId)) return;

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
        var stringId = await GetStringId(request);
        if (!Guid.TryParse(stringId, out var projectId)) return null;
        return await projectProvider.Get(ProjectId.Create(projectId));
    }

    private static async Task<string?> GetStringId(HttpRequest request)
    {
        return request.RouteValues["controller"] switch
        {
            "ProjectController" => request.Method switch
            {
               "POST" or "PUT" => await HttpRequestReader.GetStringIdFromBody(request),
               _ => null
            },
            _ => null
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
}
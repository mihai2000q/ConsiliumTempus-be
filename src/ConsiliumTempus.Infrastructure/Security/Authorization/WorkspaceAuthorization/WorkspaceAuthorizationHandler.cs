using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Infrastructure.Security.Authorization.Http;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ConsiliumTempus.Infrastructure.Security.Authorization.WorkspaceAuthorization;

public sealed class WorkspaceAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
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

        var workspace = await GetWorkspace(request, workspaceProvider);
        if (workspace is null)
        {
            context.Succeed(requirement); // let the system return the not found or validation error 
        }
        else
        {
            var userId = UserId.Create(guidUserId);
            if (IsAuthorized(requirement.AuthorizationLevel, workspace, userId)) context.Succeed(requirement);
        }
    }

    private static async Task<WorkspaceAggregate?> GetWorkspace(
        HttpRequest request,
        IWorkspaceProvider workspaceProvider)
    {
        var (stringId, idType) = await GetStringId(request);
        if (!Guid.TryParse(stringId, out var workspaceId)) return null;
        return idType switch
        {
            StringIdType.Workspace => await workspaceProvider.GetWithMemberships(WorkspaceId.Create(workspaceId)),
            StringIdType.Project => await workspaceProvider.GetByProjectWithMemberships(ProjectId.Create(workspaceId)),
            StringIdType.Empty => null,
            _ => throw new ArgumentOutOfRangeException(nameof(request))
        };
    }

    private static async Task<(string?, StringIdType)> GetStringId(HttpRequest request)
    {
        (string?, StringIdType) empty = (null, StringIdType.Empty);

        return request.RouteValues["controller"] switch
        {
            "Workspace" => request.Method switch
            {
                "GET" or "DELETE" => (HttpRequestReader.GetStringIdFromRoute(request), StringIdType.Workspace),
                "PUT" or "POST" => (await HttpRequestReader.GetStringIdFromBody(request), StringIdType.Workspace),
                _ => empty
            },
            "Project" => request.Method switch
            {
                "GET" or "DELETE" => (HttpRequestReader.GetStringIdFromRoute(request), StringIdType.Project),
                "PUT" or "POST" => (await HttpRequestReader.GetStringIdFromBody(request), StringIdType.Project),
                _ => empty
            },
            _ => empty
        };
    }

    private enum StringIdType
    {
        Empty,
        Workspace,
        Project,
    }

    private static bool IsAuthorized(
        WorkspaceAuthorizationLevel authorizationLevel,
        WorkspaceAggregate workspace,
        UserId userId)
    {
        return authorizationLevel switch
        {
            WorkspaceAuthorizationLevel.IsCollaborator => workspace.Memberships.Any(m => m.User.Id == userId),
            WorkspaceAuthorizationLevel.IsWorkspaceOwner => workspace.Owner.Id == userId,
            _ => throw new ArgumentOutOfRangeException(nameof(authorizationLevel))
        };
    }
}
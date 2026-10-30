using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Infrastructure.Security.Authorization.Http;
using ConsiliumTempus.Infrastructure.Security.Authorization.NotFound;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using ErrorOr;
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
            context.Fail(new NotFoundAuthorizationFailureReason(this, GetNotFoundError(idType)));
        }
        else
        {
            var userId = UserId.Create(guidUserId);
            if (IsAuthorized(requirement.AuthorizationLevel, workspace, userId))
                context.Succeed(requirement);
            else if (!IsCollaborator(workspace, userId)) // the user must not know that the resource exists
                context.Fail(new NotFoundAuthorizationFailureReason(this, GetNotFoundError(idType)));
        }
    }

    private static async Task<(WorkspaceAggregate?, StringIdType, bool)> GetWorkspace(
        HttpRequest request,
        IWorkspaceProvider workspaceProvider)
    {
        if (await GetStringId(request) is not var (stringId, idType)) return (null, default, false);
        if (!Guid.TryParse(stringId, out var workspaceId) || workspaceId == Guid.Empty) return (null, idType, false);
        var workspace = idType switch
        {
            StringIdType.Workspace => await workspaceProvider.GetWithMemberships(WorkspaceId.Create(workspaceId)),
            StringIdType.Project => await workspaceProvider.GetByProjectWithMemberships(ProjectId.Create(workspaceId)),
            _ => throw new ArgumentOutOfRangeException(nameof(request))
        };
        return (workspace, idType, true);
    }

    private static async Task<(string?, StringIdType)?> GetStringId(HttpRequest request)
    {
        return request.RouteValues["controller"] switch
        {
            "Workspace" => request.Method switch
            {
                "GET" or "DELETE" => (HttpRequestReader.GetStringIdFromRoute(request), StringIdType.Workspace),
                "PUT" or "POST" => (await HttpRequestReader.GetStringIdFromBody(request), StringIdType.Workspace),
                _ => null
            },
            "Project" => request.Method switch
            {
                "GET" or "DELETE" => (HttpRequestReader.GetStringIdFromRoute(request), StringIdType.Project),
                "PUT" or "POST" => (await HttpRequestReader.GetStringIdFromBody(request), StringIdType.Project),
                _ => null
            },
            _ => null
        };
    }

    private enum StringIdType
    {
        Workspace,
        Project
    }

    private static Error GetNotFoundError(StringIdType idType)
    {
        return idType switch
        {
            StringIdType.Workspace => Errors.Workspace.NotFound,
            StringIdType.Project => Errors.Project.NotFound,
            _ => throw new ArgumentOutOfRangeException(nameof(idType))
        };
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
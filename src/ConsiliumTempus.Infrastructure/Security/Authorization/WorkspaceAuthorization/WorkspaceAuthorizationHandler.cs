using ConsiliumTempus.Domain.Common.Enums;
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
        if (subUserId is null || !Guid.TryParse(subUserId, out var guidUserId)) return;

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
        var stringId = await HttpRequestReader.GetStringIdFromBody(request);
        if (stringId is null) return null;

        if (!Guid.TryParse(stringId, out var workspaceId)) return null;

        return await workspaceProvider.GetWithMemberships(WorkspaceId.Create(workspaceId));
    }

    private static bool IsAuthorized(
        WorkspaceAuthorizationLevel authorizationLevel,
        WorkspaceAggregate workspace,
        UserId userId)
    {
        return authorizationLevel switch
        {
            WorkspaceAuthorizationLevel.IsCollaborator => workspace.Memberships.Any(m => m.User.Id == userId),
            WorkspaceAuthorizationLevel.IsOwner => workspace.Owner.Id == userId,
            _ => throw new ArgumentOutOfRangeException(nameof(authorizationLevel))
        };
    }
}
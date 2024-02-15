using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Infrastructure.Authorization.Http;
using ConsiliumTempus.Infrastructure.Authorization.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ConsiliumTempus.Infrastructure.Authorization.Permission;

public sealed class PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var jwtUserId = context.User.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;

        using var scope = serviceScopeFactory.CreateScope();
        var permissionProvider = scope.ServiceProvider.GetRequiredService<IPermissionProvider>();
        var httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
        var workspaceProvider = scope.ServiceProvider.GetRequiredService<IWorkspaceProvider>();

        var request = httpContextAccessor.HttpContext?.Request;
        if (request is null) return;

        var userId = UserId.Create(jwtUserId);
        var res = await GetWorkspaceId(request, workspaceProvider);
        if (res is null) return;
        if (res.NotFound)
        {
            context.Succeed(requirement); // let the system return the not found error
        }
        else
        {
            var permissions = await permissionProvider.GetPermissions(userId, res.WorkspaceId);
            if (permissions.Contains(requirement.Permission)) context.Succeed(requirement);
        }
    }

    private static async Task<WorkspaceIdResponse?> GetWorkspaceId(
        HttpRequest request,
        IWorkspaceProvider workspaceProvider)
    {
        var stringIdRes = await GetStringWorkspaceId(request);

        if (string.IsNullOrWhiteSpace(stringIdRes.Value)) return null;
        if (!Guid.TryParse(stringIdRes.Value, out var guidId)) return null;

        var workspace = stringIdRes.Type switch
        {
            StringIdType.Project => await workspaceProvider.GetByProject(ProjectId.Create(guidId)),
            _ => await workspaceProvider.Get(WorkspaceId.Create(guidId))
        };
        
        return workspace is null
            ? new WorkspaceIdResponse(default!, true)
            : new WorkspaceIdResponse(workspace.Id, false);
    }

    private static async Task<StringIdResponse> GetStringWorkspaceId(HttpRequest request)
    {
        var type = StringIdType.Workspace;
        var value = request.RouteValues["controller"] switch
        {
            ApiControllers.Workspace => request.Method switch
            {
                HttpRequestType.GET => HttpRequestReader.GetStringIdFromRoute(request),
                HttpRequestType.PUT => await HttpRequestReader.GetStringIdFromBody(request),
                HttpRequestType.DELETE => HttpRequestReader.GetStringIdFromRoute(request),
                _ => null
            },
            ApiControllers.Project => request.Method switch
            {
                HttpRequestType.GET => HttpRequestReader.GetStringIdFromRoute(request),
                HttpRequestType.POST => await HttpRequestReader.GetPropertyFromBody(request, "workspaceId"),
                HttpRequestType.DELETE => GetStringIdFromRouteAndChangeType(request, ref type, StringIdType.Project),
                _ => null
            },
            _ => null
        };

        return new StringIdResponse(type, value);
    }

    [SuppressMessage("ReSharper", "RedundantAssignment")]
    private static string? GetStringIdFromRouteAndChangeType(
        HttpRequest request,
        ref StringIdType type,
        StringIdType newType)
    {
        type = newType;
        return HttpRequestReader.GetStringIdFromRoute(request);
    }

    private record WorkspaceIdResponse(WorkspaceId WorkspaceId, bool NotFound);

    private record StringIdResponse(StringIdType Type, string? Value);

    private enum StringIdType
    {
        Workspace,
        Project
    }
}
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
            StringIdType.ProjectSprint => await workspaceProvider.GetByProjectSprint(ProjectSprintId.Create(guidId)),
            _ => await workspaceProvider.Get(WorkspaceId.Create(guidId))
        };

        return workspace is null
            ? new WorkspaceIdResponse(default!, true)
            : new WorkspaceIdResponse(workspace.Id, false);
    }

    private static async Task<StringIdResponse> GetStringWorkspaceId(HttpRequest request)
    {
        return request.RouteValues["controller"] switch
        {
            ApiControllers.Workspace => request.Method switch
            {
                HttpRequestType.GET => new StringIdResponse(HttpRequestReader.GetStringIdFromRoute(request)),
                HttpRequestType.PUT => new StringIdResponse(await HttpRequestReader.GetStringIdFromBody(request)),
                HttpRequestType.DELETE => new StringIdResponse(HttpRequestReader.GetStringIdFromRoute(request)),
                _ => new StringIdResponse(null)
            },
            ApiControllers.Project => request.Method switch
            {
                HttpRequestType.GET => new StringIdResponse(HttpRequestReader.GetStringIdFromRoute(request)),
                HttpRequestType.POST => new StringIdResponse(
                    await HttpRequestReader.GetPropertyFromBody(request, "workspaceId")),
                HttpRequestType.DELETE => new StringIdResponse(
                    HttpRequestReader.GetStringIdFromRoute(request),
                    StringIdType.Project),
                HttpRequestType.PUT => new StringIdResponse(
                    await HttpRequestReader.GetPropertyFromBody(request, "id"), 
                    StringIdType.Project),
                _ => new StringIdResponse(null)
            },
            ApiControllers.ProjectSprint => request.Method switch
            {
                HttpRequestType.POST => new StringIdResponse(
                    await HttpRequestReader.GetPropertyFromBody(request, "projectId"),
                    StringIdType.Project),
                HttpRequestType.PUT => new StringIdResponse(
                    await HttpRequestReader.GetPropertyFromBody(request, "id"), 
                    StringIdType.ProjectSprint),
                HttpRequestType.DELETE => new StringIdResponse(
                    HttpRequestReader.GetStringIdFromRoute(request),
                    StringIdType.ProjectSprint),
                _ => new StringIdResponse(null)
            },
            _ => new StringIdResponse(null)
        };
    }

    private record WorkspaceIdResponse(WorkspaceId WorkspaceId, bool NotFound);

    private record StringIdResponse(string? Value, StringIdType Type = StringIdType.Workspace);

    private enum StringIdType
    {
        Workspace,
        Project,
        ProjectSprint
    }
}
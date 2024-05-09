using System.Text.Json;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using ConsiliumTempus.Infrastructure.UnitTests.TestData.Security.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;

namespace ConsiliumTempus.Infrastructure.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class Authorization
    {
        internal static void MockEmptyHttpRequest(
            IHttpContextAccessor httpContextAccessor,
            PermissionAuthorizationHandlerData.RequestLocation requestLocation)
        {
            switch (requestLocation)
            {
                case PermissionAuthorizationHandlerData.RequestLocation.Route:
                    httpContextAccessor
                        .HttpContext!
                        .Request
                        .RouteValues
                        .Returns(new RouteValueDictionary());
                    break;
                case PermissionAuthorizationHandlerData.RequestLocation.Query:
                    httpContextAccessor
                        .HttpContext!
                        .Request
                        .Query
                        .Returns(new QueryCollection());
                    break;
                case PermissionAuthorizationHandlerData.RequestLocation.Body:
                    var bodyStream = JsonSerializer.SerializeToUtf8Bytes(new Dictionary<string, string>());
                    httpContextAccessor
                        .HttpContext!
                        .Request
                        .Body
                        .Returns(new MemoryStream(bodyStream));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(requestLocation), requestLocation, null);
            }
        }

        internal static void MockHttpRequest(
            IHttpContextAccessor httpContextAccessor,
            PermissionAuthorizationHandlerData.RequestLocation requestLocation,
            string? id,
            string stringId)
        {
            switch (requestLocation)
            {
                case PermissionAuthorizationHandlerData.RequestLocation.Body:
                {
                    var body = new Dictionary<string, string> { [id ?? "id"] = stringId };
                    var bodyStream = JsonSerializer.SerializeToUtf8Bytes(body);
                    httpContextAccessor
                        .HttpContext!
                        .Request
                        .Body
                        .Returns(new MemoryStream(bodyStream));
                    break;
                }
                case PermissionAuthorizationHandlerData.RequestLocation.Route:
                    httpContextAccessor
                        .HttpContext!
                        .Request
                        .RouteValues
                        .Returns(new RouteValueDictionary { [id ?? "id"] = stringId });
                    break;
                case PermissionAuthorizationHandlerData.RequestLocation.Query:
                    httpContextAccessor
                        .HttpContext!
                        .Request
                        .Query
                        .Returns(new QueryCollection(
                            new Dictionary<string, StringValues> { [id ?? "id"] = stringId }));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(requestLocation), requestLocation, null);
            }
        }

        internal static void MockWorkspaceProvider(IWorkspaceProvider workspaceProvider, WorkspaceAggregate workspace)
        {
            workspaceProvider
                .Get(Arg.Any<WorkspaceId>())
                .Returns(workspace);

            workspaceProvider
                .GetByProject(Arg.Any<ProjectId>())
                .Returns(workspace);

            workspaceProvider
                .GetByProjectSprint(Arg.Any<ProjectSprintId>())
                .Returns(workspace);

            workspaceProvider
                .GetByProjectStage(Arg.Any<ProjectStageId>())
                .Returns(workspace);
        }

        internal static async Task VerifyWorkspaceProvider(
            IWorkspaceProvider workspaceProvider,
            PermissionAuthorizationHandlerData.StringIdType provider,
            string stringId)
        {
            switch (provider)
            {
                case PermissionAuthorizationHandlerData.StringIdType.Workspace:
                    await workspaceProvider
                        .Received(1)
                        .Get(Arg.Is<WorkspaceId>(wId => wId.Value.ToString() == stringId));
                    break;
                case PermissionAuthorizationHandlerData.StringIdType.Project:
                    await workspaceProvider
                        .Received(1)
                        .GetByProject(Arg.Is<ProjectId>(pId => pId.Value.ToString() == stringId));
                    break;
                case PermissionAuthorizationHandlerData.StringIdType.ProjectSprint:
                    await workspaceProvider
                        .Received(1)
                        .GetByProjectSprint(Arg.Is<ProjectSprintId>(psId => psId.Value.ToString() == stringId));
                    break;
                case PermissionAuthorizationHandlerData.StringIdType.ProjectStage:
                    await workspaceProvider
                        .Received(1)
                        .GetByProjectStage(Arg.Is<ProjectStageId>(psId => psId.Value.ToString() == stringId));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(provider), provider, null);
            }
        }
    }
}
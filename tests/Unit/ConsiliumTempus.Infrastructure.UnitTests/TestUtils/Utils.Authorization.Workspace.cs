using System.Text.Json;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using ConsiliumTempus.Infrastructure.UnitTests.TestData.Security.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ConsiliumTempus.Infrastructure.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static partial class Authorization
    {
        internal static class Workspace
        {
            internal static void MockEmptyHttpRequest(
                IHttpContextAccessor httpContextAccessor,
                WorkspaceAuthorizationHandlerData.Controller controller,
                WorkspaceAuthorizationHandlerData.Method method,
                WorkspaceAuthorizationHandlerData.RequestLocation requestLocation)
            {
                var dictionary = new RouteValueDictionary { ["controller"] = controller.ToString() };

                httpContextAccessor
                    .HttpContext!
                    .Request
                    .Method
                    .Returns(method.ToString());

                switch (requestLocation)
                {
                    case WorkspaceAuthorizationHandlerData.RequestLocation.Route:
                        break;

                    case WorkspaceAuthorizationHandlerData.RequestLocation.Body:
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

                httpContextAccessor
                    .HttpContext!
                    .Request
                    .RouteValues
                    .Returns(dictionary);
            }


            internal static void MockHttpRequest(
                IHttpContextAccessor httpContextAccessor,
                WorkspaceAuthorizationHandlerData.Controller controller,
                WorkspaceAuthorizationHandlerData.Method method,
                WorkspaceAuthorizationHandlerData.RequestLocation requestLocation,
                string stringId)
            {
                const string idName = "id";

                var dictionary = new RouteValueDictionary { ["controller"] = controller.ToString() };

                httpContextAccessor
                    .HttpContext!
                    .Request
                    .Method
                    .Returns(method.ToString());

                switch (requestLocation)
                {
                    case WorkspaceAuthorizationHandlerData.RequestLocation.Body:
                        var body = new Dictionary<string, string> { [idName] = stringId };
                        var bodyStream = JsonSerializer.SerializeToUtf8Bytes(body);
                        httpContextAccessor
                            .HttpContext!
                            .Request
                            .Body
                            .Returns(new MemoryStream(bodyStream));
                        break;

                    case WorkspaceAuthorizationHandlerData.RequestLocation.Route:
                        dictionary[idName] = stringId;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(requestLocation), requestLocation, null);
                }

                httpContextAccessor
                    .HttpContext!
                    .Request
                    .RouteValues
                    .Returns(dictionary);
            }

            internal static void MockWorkspaceProvider(IWorkspaceProvider workspaceProvider,
                WorkspaceAggregate workspace)
            {
                workspaceProvider
                    .GetWithMemberships(Arg.Any<WorkspaceId>())
                    .Returns(workspace);

                workspaceProvider
                    .GetByProjectWithMemberships(Arg.Any<ProjectId>())
                    .Returns(workspace);
            }

            internal static async Task VerifyWorkspaceProvider(
                IWorkspaceProvider workspaceProvider,
                WorkspaceAuthorizationHandlerData.StringIdType provider,
                string stringId)
            {
                switch (provider)
                {
                    case WorkspaceAuthorizationHandlerData.StringIdType.Workspace:
                        await workspaceProvider
                            .Received(1)
                            .GetWithMemberships(Arg.Is<WorkspaceId>(wId => wId.Value.ToString() == stringId));
                        break;

                    case WorkspaceAuthorizationHandlerData.StringIdType.Project:
                        await workspaceProvider
                            .Received(1)
                            .GetByProjectWithMemberships(Arg.Is<ProjectId>(pId => pId.Value.ToString() == stringId));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(provider), provider, null);
                }
            }
        }
    }
}
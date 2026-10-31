using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using Microsoft.AspNetCore.Http;
using ConsiliumTempus.Infrastructure.Security.Authorization.Targets;

namespace ConsiliumTempus.Infrastructure.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static partial class Authorization
    {
        internal static class Workspace
        {
            internal static void MockEmptyHttpRequest(
                IHttpContextAccessor httpContextAccessor,
                Controller controller,
                Method method,
                IdLocation requestLocation)
            {
                MockRequest(httpContextAccessor, controller, null, method, requestLocation, "id", null);
            }

            internal static void MockHttpRequest(
                IHttpContextAccessor httpContextAccessor,
                Controller controller,
                Method method,
                IdLocation requestLocation,
                string stringId)
            {
                MockRequest(httpContextAccessor, controller, null, method, requestLocation, "id", stringId);
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
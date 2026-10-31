using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using ConsiliumTempus.Infrastructure.UnitTests.TestData.Security.Authorization;
using Microsoft.AspNetCore.Http;
using ConsiliumTempus.Infrastructure.Security.Authorization.Targets;

namespace ConsiliumTempus.Infrastructure.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static partial class Authorization
    {
        internal static class Permission
        {
            internal static void MockEmptyHttpRequest(
                IHttpContextAccessor httpContextAccessor,
                Permissions permission,
                IdLocation requestLocation)
            {
                var (controller, action, method) = PermissionAuthorizationHandlerData.Endpoints[permission];
                MockRequest(httpContextAccessor, controller, action, method, requestLocation, "id", null);
            }

            internal static void MockHttpRequest(
                IHttpContextAccessor httpContextAccessor,
                Permissions permission,
                IdLocation requestLocation,
                string? idName,
                string stringId)
            {
                var (controller, action, method) = PermissionAuthorizationHandlerData.Endpoints[permission];
                MockRequest(httpContextAccessor, controller, action, method, requestLocation, idName ?? "id", stringId);
            }

            internal static void MockWorkspaceProvider(IWorkspaceProvider workspaceProvider,
                WorkspaceAggregate workspace)
            {
                workspaceProvider
                    .Get(Arg.Any<WorkspaceId>())
                    .Returns(workspace);

                workspaceProvider
                    .GetByCustomFieldSetup(Arg.Any<CustomFieldSetupId>())
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

                workspaceProvider
                    .GetByProjectTask(Arg.Any<ProjectTaskId>())
                    .Returns(workspace);
            }

            internal static async Task VerifyWorkspaceProvider(
                IWorkspaceProvider workspaceProvider,
                PermissionAuthorizationHandlerData.StringIdType provider,
                string stringId)
            {
                switch (provider)
                {
                    case PermissionAuthorizationHandlerData.StringIdType.CustomFieldSetup:
                        await workspaceProvider
                            .Received(1)
                            .GetByCustomFieldSetup(Arg.Is<CustomFieldSetupId>(cfsId => cfsId.Value.ToString() == stringId));
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

                    case PermissionAuthorizationHandlerData.StringIdType.ProjectTask:
                        await workspaceProvider
                            .Received(1)
                            .GetByProjectTask(Arg.Is<ProjectTaskId>(ptId => ptId.Value.ToString() == stringId));
                        break;

                    case PermissionAuthorizationHandlerData.StringIdType.Workspace:
                        await workspaceProvider
                            .Received(1)
                            .Get(Arg.Is<WorkspaceId>(wId => wId.Value.ToString() == stringId));
                        break;
                    
                    default:
                        throw new ArgumentOutOfRangeException(nameof(provider), provider, null);
                }
            }
        }
    }
}
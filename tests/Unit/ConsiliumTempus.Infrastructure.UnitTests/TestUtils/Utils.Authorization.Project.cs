using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Infrastructure.Extensions;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using Microsoft.AspNetCore.Http;
using ConsiliumTempus.Infrastructure.Security.Authorization.Targets;

namespace ConsiliumTempus.Infrastructure.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static partial class Authorization
    {
        internal static class Project
        {
            internal static void MockEmptyHttpRequest(
                IHttpContextAccessor httpContextAccessor,
                IdLocation requestLocation,
                Controller controller,
                Action requestAction)
            {
                MockRequest(httpContextAccessor, controller, requestAction, GetHttpMethod(requestLocation),
                    requestLocation, "id", null);
            }

            internal static void MockHttpRequest(
                IHttpContextAccessor httpContextAccessor,
                IdLocation requestLocation,
                Type? idType,
                Controller controller,
                Action requestAction,
                string stringId)
            {
                MockRequest(httpContextAccessor, controller, requestAction, GetHttpMethod(requestLocation),
                    requestLocation, idType?.ToCamelId() ?? "id", stringId);
            }

            private static Method GetHttpMethod(IdLocation requestLocation) =>
                requestLocation == IdLocation.Body ? Method.POST : Method.GET;

            internal static void MockProjectProvider(IProjectProvider projectProvider, ProjectAggregate project)
            {
                projectProvider
                    .Get(Arg.Any<ProjectId>())
                    .Returns(project);

                projectProvider
                    .GetByCustomFieldSetup(Arg.Any<CustomFieldSetupId>())
                    .Returns(project);

                projectProvider
                    .GetByProjectSprint(Arg.Any<ProjectSprintId>())
                    .Returns(project);

                projectProvider
                    .GetByProjectStage(Arg.Any<ProjectStageId>())
                    .Returns(project);

                projectProvider
                    .GetByProjectTask(Arg.Any<ProjectTaskId>())
                    .Returns(project);
            }

            internal static async Task VerifyProjectProvider(
                IProjectProvider projectProvider,
                TargetKind provider,
                string stringId)
            {
                switch (provider)
                {
                    case TargetKind.CustomFieldSetup:
                        await projectProvider
                            .Received(1)
                            .GetByCustomFieldSetup(Arg.Is<CustomFieldSetupId>(cfsId => cfsId.Value.ToString() == stringId));
                        break;

                    case TargetKind.Project:
                        await projectProvider
                            .Received(1)
                            .Get(Arg.Is<ProjectId>(pId => pId.Value.ToString() == stringId));
                        break;

                    case TargetKind.ProjectSprint:
                        await projectProvider
                            .Received(1)
                            .GetByProjectSprint(Arg.Is<ProjectSprintId>(psId => psId.Value.ToString() == stringId));
                        break;

                    case TargetKind.ProjectStage:
                        await projectProvider
                            .Received(1)
                            .GetByProjectStage(Arg.Is<ProjectStageId>(psId => psId.Value.ToString() == stringId));
                        break;

                    case TargetKind.ProjectTask:
                        await projectProvider
                            .Received(1)
                            .GetByProjectTask(Arg.Is<ProjectTaskId>(ptId => ptId.Value.ToString() == stringId));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(provider), provider, null);
                }
            }
        }
    }
}
using System.Text.Json;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Infrastructure.Extensions;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using ConsiliumTempus.Infrastructure.UnitTests.TestData.Security.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;

namespace ConsiliumTempus.Infrastructure.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static partial class Authorization
    {
        internal static class Project
        {
            internal static void MockEmptyHttpRequest(
                IHttpContextAccessor httpContextAccessor,
                ProjectAuthorizationHandlerData.RequestLocation requestLocation,
                ProjectAuthorizationHandlerData.Controller controller,
                string requestAction)
            {
                var dictionary = new RouteValueDictionary
                {
                    ["controller"] = controller.ToString(),
                    ["action"] = requestAction
                };

                switch (requestLocation)
                {
                    case ProjectAuthorizationHandlerData.RequestLocation.Route:
                        break;

                    case ProjectAuthorizationHandlerData.RequestLocation.Query:
                        httpContextAccessor
                            .HttpContext!
                            .Request
                            .Query
                            .Returns(new QueryCollection());
                        break;

                    case ProjectAuthorizationHandlerData.RequestLocation.Body:
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
                ProjectAuthorizationHandlerData.RequestLocation requestLocation,
                Type? idType,
                ProjectAuthorizationHandlerData.Controller controller,
                string requestAction,
                string stringId)
            {
                var idName = idType?.ToCamelId() ?? "id";

                var dictionary = new RouteValueDictionary
                {
                    ["controller"] = controller.ToString(),
                    ["action"] = requestAction
                };

                switch (requestLocation)
                {
                    case ProjectAuthorizationHandlerData.RequestLocation.Body:
                        var body = new Dictionary<string, string> { [idName] = stringId };
                        var bodyStream = JsonSerializer.SerializeToUtf8Bytes(body);
                        httpContextAccessor
                            .HttpContext!
                            .Request
                            .Body
                            .Returns(new MemoryStream(bodyStream));
                        break;

                    case ProjectAuthorizationHandlerData.RequestLocation.Route:
                        dictionary[idName] = stringId;
                        break;

                    case ProjectAuthorizationHandlerData.RequestLocation.Query:
                        httpContextAccessor
                            .HttpContext!
                            .Request
                            .Query
                            .Returns(new QueryCollection(
                                new Dictionary<string, StringValues> { [idName] = stringId }));
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

            internal static void MockProjectProvider(IProjectProvider projectProvider, ProjectAggregate project)
            {
                projectProvider
                    .Get(Arg.Any<ProjectId>())
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
                ProjectAuthorizationHandlerData.StringIdType provider,
                string stringId)
            {
                switch (provider)
                {
                    case ProjectAuthorizationHandlerData.StringIdType.Project:
                        await projectProvider
                            .Received(1)
                            .Get(Arg.Is<ProjectId>(pId => pId.Value.ToString() == stringId));
                        break;

                    case ProjectAuthorizationHandlerData.StringIdType.ProjectSprint:
                        await projectProvider
                            .Received(1)
                            .GetByProjectSprint(Arg.Is<ProjectSprintId>(psId => psId.Value.ToString() == stringId));
                        break;

                    case ProjectAuthorizationHandlerData.StringIdType.ProjectStage:
                        await projectProvider
                            .Received(1)
                            .GetByProjectStage(Arg.Is<ProjectStageId>(psId => psId.Value.ToString() == stringId));
                        break;

                    case ProjectAuthorizationHandlerData.StringIdType.ProjectTask:
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
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Infrastructure.Security.Authorization.Targets;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;

namespace ConsiliumTempus.Infrastructure.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static partial class Authorization
    {
        /// <summary>
        /// The HTTP methods of the endpoints
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        internal enum Method
        {
            GET,
            POST,
            PUT,
            PATCH,
            DELETE
        }

        /// <summary>
        /// The controllers that have authorized endpoints, named as the routing names them
        /// </summary>
        internal enum Controller
        {
            CustomFieldSetup,
            Project,
            ProjectSprint,
            ProjectTask,
            Workspace
        }

        /// <summary>
        /// The actions of the endpoints that the handler data goes through, named as the routing names them
        /// </summary>
        internal enum Action
        {
            AddAllowedMember,
            AddStage,
            AddStatus,
            AddToProject,
            Create,
            CreateOnProject,
            CreateOnWorkspace,
            Delete,
            Get,
            GetAllowedMembers,
            GetCollaborators,
            GetCollection,
            GetCollectionFromProject,
            GetCollectionFromWorkspace,
            GetInvitations,
            GetOverview,
            GetStages,
            GetStatuses,
            InviteCollaborator,
            KickCollaborator,
            LeavePrivate,
            MakeGlobal,
            Move,
            MoveStage,
            RemoveAllowedMember,
            RemoveFromProject,
            RemoveStage,
            RemoveStatus,
            Update,
            UpdateCollaborator,
            UpdateCustomField,
            UpdateFavorites,
            UpdateIsCompleted,
            UpdateOverview,
            UpdateOwner,
            UpdatePrivacy,
            UpdateStage,
            UpdateStatus
        }

        /// <summary>
        /// Describes the request that the authorization handlers read: the endpoint it goes to
        /// and where it carries the id, which is left out when the <paramref name="stringId"/> is null
        /// </summary>
        internal static void MockRequest(
            IHttpContextAccessor httpContextAccessor,
            Controller controller,
            Action? action,
            Method method,
            IdLocation location,
            string idName,
            string? stringId)
        {
            var request = httpContextAccessor.HttpContext!.Request;

            var routeValues = new RouteValueDictionary { ["controller"] = controller.ToString() };
            if (action is not null) routeValues["action"] = action.ToString();

            switch (location)
            {
                case IdLocation.Route:
                    if (stringId is not null) routeValues[idName] = stringId;
                    break;

                case IdLocation.Query:
                    request.Query.Returns(stringId is null
                        ? new QueryCollection()
                        : new QueryCollection(new Dictionary<string, StringValues> { [idName] = stringId }));
                    break;

                case IdLocation.Body:
                    var body = new Dictionary<string, string>();
                    if (stringId is not null) body[idName] = stringId;
                    request.Body.Returns(new MemoryStream(JsonSerializer.SerializeToUtf8Bytes(body)));
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(location), location, null);
            }

            request.Method.Returns(method.ToString());
            request.RouteValues.Returns(routeValues);
        }

        /// <summary>
        /// The error that the authorization fails with when the entity of that kind does not exist (or is hidden)
        /// </summary>
        internal static Error GetNotFoundError(TargetKind kind)
        {
            return kind switch
            {
                TargetKind.CustomFieldSetup => Errors.CustomFieldSetup.NotFound,
                TargetKind.Project => Errors.Project.NotFound,
                TargetKind.ProjectSprint => Errors.ProjectSprint.NotFound,
                TargetKind.ProjectStage => Errors.ProjectStage.NotFound,
                TargetKind.ProjectTask => Errors.ProjectTask.NotFound,
                TargetKind.Workspace => Errors.Workspace.NotFound,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };
        }
    }
}

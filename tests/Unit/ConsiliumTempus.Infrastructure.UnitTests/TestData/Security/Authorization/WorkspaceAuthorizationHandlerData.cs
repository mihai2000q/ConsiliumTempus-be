using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Infrastructure.UnitTests.TestData.Security.Authorization;

public static class WorkspaceAuthorizationHandlerData
{
    public enum Controller
    {
        Workspace,
        Project
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum Method
    {
        GET,
        POST,
    }

    public enum RequestLocation
    {
        Route,
        Body
    }

    public enum StringIdType
    {
        Workspace,
        Project
    }

    internal class GetAuthorizationLevels
        : TheoryData<WorkspaceAuthorizationLevel, Controller, Method, RequestLocation, StringIdType>
    {
        public GetAuthorizationLevels()
        {
            var level = WorkspaceAuthorizationLevel.IsCollaborator;
            Add(level, Controller.Workspace, Method.POST, RequestLocation.Body, StringIdType.Workspace);

            Add(level, Controller.Project, Method.POST, RequestLocation.Body, StringIdType.Project);
            Add(level, Controller.Project, Method.GET, RequestLocation.Route, StringIdType.Project);

            level = WorkspaceAuthorizationLevel.IsWorkspaceOwner;
            Add(level, Controller.Workspace, Method.POST, RequestLocation.Body, StringIdType.Workspace);
        }
    }
}
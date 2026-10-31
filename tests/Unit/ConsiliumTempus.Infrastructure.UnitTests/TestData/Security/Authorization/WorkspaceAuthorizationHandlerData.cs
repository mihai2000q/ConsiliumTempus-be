using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Infrastructure.Security.Authorization.Targets;
using Controller = ConsiliumTempus.Infrastructure.UnitTests.TestUtils.Utils.Authorization.Controller;
using Method = ConsiliumTempus.Infrastructure.UnitTests.TestUtils.Utils.Authorization.Method;

namespace ConsiliumTempus.Infrastructure.UnitTests.TestData.Security.Authorization;

public static class WorkspaceAuthorizationHandlerData
{
    internal class GetAuthorizationLevels
        : TheoryData<WorkspaceAuthorizationLevel, Controller, Method, IdLocation, TargetKind>
    {
        public GetAuthorizationLevels()
        {
            var level = WorkspaceAuthorizationLevel.IsCollaborator;
            Add(level, Controller.Workspace, Method.DELETE, IdLocation.Route, TargetKind.Workspace);

            Add(level, Controller.Project, Method.GET, IdLocation.Route, TargetKind.Project);
            Add(level, Controller.Project, Method.POST, IdLocation.Body, TargetKind.Project);
            Add(level, Controller.Project, Method.PUT, IdLocation.Body, TargetKind.Project);
            Add(level, Controller.Project, Method.DELETE, IdLocation.Route, TargetKind.Project);

            level = WorkspaceAuthorizationLevel.IsWorkspaceOwner;
            Add(level, Controller.Workspace, Method.PUT, IdLocation.Body, TargetKind.Workspace);
        }
    }
}
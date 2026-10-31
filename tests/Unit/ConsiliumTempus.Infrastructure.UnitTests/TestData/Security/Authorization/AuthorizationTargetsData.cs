using ConsiliumTempus.Infrastructure.Security.Authorization.Targets;
using Action = ConsiliumTempus.Infrastructure.UnitTests.TestUtils.Utils.Authorization.Action;
using Controller = ConsiliumTempus.Infrastructure.UnitTests.TestUtils.Utils.Authorization.Controller;
using Method = ConsiliumTempus.Infrastructure.UnitTests.TestUtils.Utils.Authorization.Method;

namespace ConsiliumTempus.Infrastructure.UnitTests.TestData.Security.Authorization;

public static class AuthorizationTargetsData
{
    internal class GetOwnTargets : TheoryData<Controller, TargetKind>
    {
        public GetOwnTargets()
        {
            Add(Controller.CustomFieldSetup, TargetKind.CustomFieldSetup);
            Add(Controller.Project, TargetKind.Project);
            Add(Controller.ProjectSprint, TargetKind.ProjectSprint);
            Add(Controller.ProjectTask, TargetKind.ProjectTask);
            Add(Controller.Workspace, TargetKind.Workspace);
        }
    }

    internal class GetHttpMethods : TheoryData<Method, IdLocation>
    {
        public GetHttpMethods()
        {
            Add(Method.GET, IdLocation.Route);
            Add(Method.DELETE, IdLocation.Route);
            Add(Method.POST, IdLocation.Body);
            Add(Method.PUT, IdLocation.Body);
            Add(Method.PATCH, IdLocation.Body);
        }
    }

    internal class GetParentTargets : TheoryData<Controller, Action, Method, TargetKind, IdLocation, string>
    {
        public GetParentTargets()
        {
            // Custom Field Setup Controller
            Add(Controller.CustomFieldSetup, Action.GetCollectionFromWorkspace, Method.GET, TargetKind.Workspace, IdLocation.Route, "workspaceId");
            Add(Controller.CustomFieldSetup, Action.GetCollectionFromProject, Method.GET, TargetKind.Project, IdLocation.Route, "projectId");
            Add(Controller.CustomFieldSetup, Action.CreateOnWorkspace, Method.POST, TargetKind.Workspace, IdLocation.Body, "workspaceId");
            Add(Controller.CustomFieldSetup, Action.CreateOnProject, Method.POST, TargetKind.Project, IdLocation.Body, "projectId");

            // Project Controller
            Add(Controller.Project, Action.GetCollection, Method.GET, TargetKind.Workspace, IdLocation.Query, "workspaceId");
            Add(Controller.Project, Action.Create, Method.POST, TargetKind.Workspace, IdLocation.Body, "workspaceId");

            // Project Sprint Controller
            Add(Controller.ProjectSprint, Action.GetCollection, Method.GET, TargetKind.Project, IdLocation.Query, "projectId");
            Add(Controller.ProjectSprint, Action.Create, Method.POST, TargetKind.Project, IdLocation.Body, "projectId");

            // Project Task Controller
            Add(Controller.ProjectTask, Action.GetCollection, Method.GET, TargetKind.ProjectStage, IdLocation.Query, "projectStageId");
            Add(Controller.ProjectTask, Action.Create, Method.POST, TargetKind.ProjectStage, IdLocation.Body, "projectStageId");

            // Workspace Controller
            Add(Controller.Workspace, Action.GetInvitations, Method.GET, TargetKind.Workspace, IdLocation.Query, "workspaceId");
        }
    }

    internal class GetProjectTargets : TheoryData<Action, Method, IdLocation>
    {
        public GetProjectTargets()
        {
            Add(Action.AddToProject, Method.POST, IdLocation.Body);
            Add(Action.RemoveFromProject, Method.DELETE, IdLocation.Route);
        }
    }

    /// <summary>
    /// The controllers (as the routing names them) that have no authorized endpoints
    /// </summary>
    internal class GetControllersWithoutTarget : TheoryData<string?>
    {
        public GetControllersWithoutTarget()
        {
            Add(null);
            Add("");
            Add("Authentication");
            Add("User");
        }
    }

    /// <summary>
    /// The numbers that would parse to the members of an enum, but are not the name of any of them
    /// </summary>
    internal class GetNumbers : TheoryData<string>
    {
        public GetNumbers()
        {
            Add("0");
            Add("1");
            Add("99");
        }
    }

    internal class GetHttpMethodsWithoutTarget : TheoryData<string>
    {
        public GetHttpMethodsWithoutTarget()
        {
            Add("");
            Add("OPTIONS");
            Add("HEAD");
        }
    }
}

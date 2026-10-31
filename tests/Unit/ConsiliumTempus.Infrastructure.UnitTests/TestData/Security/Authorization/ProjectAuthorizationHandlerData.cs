using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Infrastructure.Security.Authorization.Targets;
using Action = ConsiliumTempus.Infrastructure.UnitTests.TestUtils.Utils.Authorization.Action;
using Controller = ConsiliumTempus.Infrastructure.UnitTests.TestUtils.Utils.Authorization.Controller;

namespace ConsiliumTempus.Infrastructure.UnitTests.TestData.Security.Authorization;

public static class ProjectAuthorizationHandlerData
{
    internal class GetIsAllowed
        : TheoryData<ProjectAuthorizationLevel, IdLocation, Type?, Controller, Action, TargetKind>
    {
        public GetIsAllowed()
        {
            const ProjectAuthorizationLevel level = ProjectAuthorizationLevel.IsAllowed;

            // Custom Field Setup Controller
            var controller = Controller.CustomFieldSetup;
            var requestLocation = IdLocation.Route;
            var stringIdType = TargetKind.CustomFieldSetup;
            Add(level, requestLocation, null, controller, Action.Get, stringIdType);
            Add(level, requestLocation, null, controller, Action.Delete, stringIdType);
            Add(level, requestLocation, typeof(ProjectAggregate), controller, Action.GetCollectionFromProject, TargetKind.Project);
            Add(level, requestLocation, typeof(ProjectAggregate), controller, Action.RemoveFromProject, TargetKind.Project);

            requestLocation = IdLocation.Body;
            Add(level, requestLocation, typeof(ProjectAggregate), controller, Action.CreateOnProject, TargetKind.Project);
            Add(level, requestLocation, typeof(ProjectAggregate), controller, Action.AddToProject, TargetKind.Project);
            Add(level, requestLocation, null, controller, Action.Update, stringIdType);
            Add(level, requestLocation, null, controller, Action.MakeGlobal, stringIdType);

            // Project Controller
            controller = Controller.Project;
            requestLocation = IdLocation.Route;
            stringIdType = TargetKind.Project;
            Add(level, requestLocation, null, controller, Action.Get, stringIdType);
            Add(level, requestLocation, null, controller, Action.GetOverview, stringIdType);
            Add(level, requestLocation, null, controller, Action.GetAllowedMembers, stringIdType);
            Add(level, requestLocation, null, controller, Action.GetStatuses, stringIdType);
            Add(level, requestLocation, null, controller, Action.Delete, stringIdType);
            Add(level, requestLocation, null, controller, Action.LeavePrivate, stringIdType);
            Add(level, requestLocation, null, controller, Action.RemoveStatus, stringIdType);

            requestLocation = IdLocation.Body;
            Add(level, requestLocation, null, controller, Action.AddStatus, stringIdType);
            Add(level, requestLocation, null, controller, Action.Update, stringIdType);
            Add(level, requestLocation, null, controller, Action.UpdateFavorites, stringIdType);
            Add(level, requestLocation, null, controller, Action.UpdateOverview, stringIdType);
            Add(level, requestLocation, null, controller, Action.UpdateStatus, stringIdType);

            // Project Sprint Controller
            controller = Controller.ProjectSprint;
            requestLocation = IdLocation.Route;
            stringIdType = TargetKind.ProjectSprint;
            Add(level, requestLocation, null, controller, Action.Get, stringIdType);
            Add(level, requestLocation, null, controller, Action.GetStages, stringIdType);
            Add(level, requestLocation, null, controller, Action.Delete, stringIdType);
            Add(level, requestLocation, null, controller, Action.RemoveStage, stringIdType);

            requestLocation = IdLocation.Query;
            Add(level, requestLocation, typeof(ProjectAggregate), controller, Action.GetCollection, TargetKind.Project);

            requestLocation = IdLocation.Body;
            Add(level, requestLocation, typeof(ProjectAggregate), controller, Action.Create, TargetKind.Project);
            Add(level, requestLocation, null, controller, Action.AddStage, stringIdType);
            Add(level, requestLocation, null, controller, Action.Update, stringIdType);
            Add(level, requestLocation, null, controller, Action.MoveStage, stringIdType);
            Add(level, requestLocation, null, controller, Action.UpdateStage, stringIdType);

            // Project Task Controller
            controller = Controller.ProjectTask;
            requestLocation = IdLocation.Route;
            stringIdType = TargetKind.ProjectTask;
            Add(level, requestLocation, null, controller, Action.Get, stringIdType);
            Add(level, requestLocation, null, controller, Action.Delete, stringIdType);

            requestLocation = IdLocation.Query;
            Add(level, requestLocation, typeof(ProjectStage), controller, Action.GetCollection, TargetKind.ProjectStage);

            requestLocation = IdLocation.Body;
            Add(level, requestLocation, typeof(ProjectStage), controller, Action.Create, TargetKind.ProjectStage);
            Add(level, requestLocation, null, controller, Action.Move, stringIdType);
            Add(level, requestLocation, null, controller, Action.Update, stringIdType);
            Add(level, requestLocation, null, controller, Action.UpdateCustomField, stringIdType);
            Add(level, requestLocation, null, controller, Action.UpdateIsCompleted, stringIdType);
            Add(level, requestLocation, null, controller, Action.UpdateOverview, stringIdType);
        }
    }

    internal class GetIsOwner
        : TheoryData<ProjectAuthorizationLevel, IdLocation, Type?, Controller, Action, TargetKind>
    {
        public GetIsOwner()
        {
            // Project Authorization Level: Is Project Owner
            const ProjectAuthorizationLevel level = ProjectAuthorizationLevel.IsProjectOwner;
            const Controller controller = Controller.Project;
            const TargetKind stringIdType = TargetKind.Project;

            Add(level, IdLocation.Body, null, controller, Action.AddAllowedMember, stringIdType);
            Add(level, IdLocation.Body, null, controller, Action.UpdateOwner, stringIdType);
            Add(level, IdLocation.Body, null, controller, Action.UpdatePrivacy, stringIdType);
            Add(level, IdLocation.Route, null, controller, Action.RemoveAllowedMember, stringIdType);
        }
    }
}
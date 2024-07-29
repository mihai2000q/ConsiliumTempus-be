using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectSprint.Entities;

namespace ConsiliumTempus.Infrastructure.UnitTests.TestData.Security.Authorization;

public static class ProjectAuthorizationHandlerData
{
    public enum RequestLocation
    {
        Route,
        Query,
        Body
    }
    
    public enum Controller
    {
        ProjectController,
        ProjectSprintController,
        ProjectTaskController
    }

    public enum StringIdType
    {
        Project,
        ProjectSprint,
        ProjectStage,
        ProjectTask
    }

    public class GetAuthorizationLevels 
        : TheoryData<ProjectAuthorizationLevel, RequestLocation, Type?, Controller, string, StringIdType>
    {
        public GetAuthorizationLevels()
        {
            const ProjectAuthorizationLevel level = ProjectAuthorizationLevel.IsAllowed;

            // Project Controller
            var controller = Controller.ProjectController;
            var requestLocation = RequestLocation.Route;
            var stringIdType = StringIdType.Project;
            Add(level, requestLocation, null, controller, "Get", stringIdType);
            Add(level, requestLocation, null, controller, "GetOverview", stringIdType);
            Add(level, requestLocation, null, controller, "GetStatuses", stringIdType);
            Add(level, requestLocation, null, controller, "Delete", stringIdType);
            Add(level, requestLocation, null, controller, "RemoveStatus", stringIdType);

            requestLocation = RequestLocation.Body;
            Add(level, requestLocation, null, controller, "AddStatus", stringIdType);
            Add(level, requestLocation, null, controller, "Update", stringIdType);
            Add(level, requestLocation, null, controller, "UpdateFavorites", stringIdType);
            Add(level, requestLocation, null, controller, "UpdateOverview", stringIdType);
            Add(level, requestLocation, null, controller, "UpdateStatus", stringIdType);
            
            // Project Sprint Controller
            controller = Controller.ProjectSprintController;
            requestLocation = RequestLocation.Route;
            stringIdType = StringIdType.ProjectSprint;
            Add(level, requestLocation, null, controller, "Get", stringIdType);
            Add(level, requestLocation, null, controller, "GetStages", stringIdType);
            Add(level, requestLocation, null, controller, "Delete", stringIdType);
            Add(level, requestLocation, null, controller, "RemoveStage", stringIdType);
            
            requestLocation = RequestLocation.Query;
            Add(level, requestLocation, typeof(ProjectAggregate), controller, "GetCollection", StringIdType.Project);

            requestLocation = RequestLocation.Body;
            Add(level, requestLocation, typeof(ProjectAggregate), controller, "Create", StringIdType.Project);
            Add(level, requestLocation, null, controller, "AddStage", stringIdType);
            Add(level, requestLocation, null, controller, "Update", stringIdType);
            Add(level, requestLocation, null, controller, "MoveStage", stringIdType);
            Add(level, requestLocation, null, controller, "UpdateStage", stringIdType);
            
            // Project Task Controller
            controller = Controller.ProjectTaskController;
            requestLocation = RequestLocation.Route;
            stringIdType = StringIdType.ProjectTask;
            Add(level, requestLocation, null, controller, "Get", stringIdType);
            Add(level, requestLocation, null, controller, "Delete", stringIdType);
            
            requestLocation = RequestLocation.Query;
            Add(level, requestLocation, typeof(ProjectStage), controller, "GetCollection", StringIdType.ProjectStage);

            requestLocation = RequestLocation.Body;
            Add(level, requestLocation, typeof(ProjectStage), controller, "Create", StringIdType.ProjectStage);
            Add(level, requestLocation, null, controller, "Move", stringIdType);
            Add(level, requestLocation, null, controller, "Update", stringIdType);
            Add(level, requestLocation, null, controller, "UpdateIsCompleted", stringIdType);
            Add(level, requestLocation, null, controller, "UpdateOverview", stringIdType);
        }
    }
}
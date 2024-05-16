using ConsiliumTempus.Api.Contracts.Project.Entities.Stage.Create;
using ConsiliumTempus.Api.Contracts.Project.Entities.Stage.Delete;
using ConsiliumTempus.Api.Contracts.Project.Entities.Stage.Update;
using ConsiliumTempus.Common.UnitTests.TestConstants;

namespace ConsiliumTempus.Common.UnitTests.Project.Entities.Stage;

public static class ProjectStageRequestFactory
{
    public static CreateProjectStageRequest CreateCreateProjectStageRequest(
        Guid? projectSprintId = null,
        string name = Constants.ProjectStage.Name,
        bool onTop = false)
    {
        return new CreateProjectStageRequest(
            projectSprintId ?? Guid.NewGuid(),
            name,
            onTop);
    }

    public static UpdateProjectStageRequest CreateUpdateProjectStageRequest(
        Guid? id = null,
        string name = Constants.ProjectStage.Name)
    {
        return new UpdateProjectStageRequest(
            id ?? Guid.NewGuid(),
            name);
    }
    
    public static DeleteProjectStageRequest CreateDeleteProjectStageRequest(Guid? id = null)
    {
        return new DeleteProjectStageRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }
}
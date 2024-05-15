using ConsiliumTempus.Api.Contracts.Project.Entities.Stage.Create;
using ConsiliumTempus.Api.Contracts.Project.Entities.Stage.Delete;
using ConsiliumTempus.Api.Contracts.Project.Entities.Stage.Update;
using ConsiliumTempus.Common.IntegrationTests.TestConstants;

namespace ConsiliumTempus.Common.IntegrationTests.Project.Entities.Stage;

public static class ProjectStageRequestFactory
{
    public static CreateProjectStageRequest CreateCreateProjectStageRequest(
        Guid? projectSprintId = null,
        string name = Constants.ProjectStage.Name)
    {
        return new CreateProjectStageRequest(
            projectSprintId ?? Guid.NewGuid(),
            name);
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
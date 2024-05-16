using ConsiliumTempus.Api.Contracts.ProjectTask.Create;
using ConsiliumTempus.Api.Contracts.ProjectTask.Delete;
using ConsiliumTempus.Api.Contracts.ProjectTask.Get;
using ConsiliumTempus.Api.Contracts.ProjectTask.GetCollection;
using ConsiliumTempus.Common.UnitTests.TestConstants;

namespace ConsiliumTempus.Common.UnitTests.ProjectTask;

public static class ProjectTaskRequestFactory
{
    public static GetProjectTaskRequest CreateGetProjectTaskRequest(
        Guid? id = null)
    {
        return new GetProjectTaskRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }

    public static GetCollectionProjectTaskRequest CreateGetCollectionProjectTaskRequest(
        Guid? projectStageId = null)
    {
        return new GetCollectionProjectTaskRequest
        {
            ProjectStageId = projectStageId ?? Guid.NewGuid()
        };
    }

    public static CreateProjectTaskRequest CreateCreateProjectTaskRequest(
        Guid? projectStageId = null,
        string name = Constants.ProjectTask.Name,
        bool onTop = false)
    {
        return new CreateProjectTaskRequest(
            projectStageId ?? Guid.NewGuid(),
            name,
            onTop);
    }
    
    public static DeleteProjectTaskRequest CreateDeleteProjectTaskRequest(
        Guid? id = null,
        Guid? projectStageId = null)
    {
        return new DeleteProjectTaskRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }
}
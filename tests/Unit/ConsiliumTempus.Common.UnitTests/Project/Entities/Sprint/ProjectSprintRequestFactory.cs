using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Create;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Delete;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Get;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.GetCollection;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Update;
using ConsiliumTempus.Common.UnitTests.TestConstants;

namespace ConsiliumTempus.Common.UnitTests.Project.Entities.Sprint;

public static class ProjectSprintRequestFactory
{
    public static GetProjectSprintRequest CreateGetProjectSprintRequest(Guid? id = null)
    {
        return new GetProjectSprintRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }

    public static GetCollectionProjectSprintRequest CreateGetCollectionProjectSprintRequest(
        Guid? id = null)
    {
        return new GetCollectionProjectSprintRequest
        {
            ProjectId = id ?? Guid.NewGuid()
        };
    }

    public static CreateProjectSprintRequest CreateCreateProjectSprintRequest(
        Guid? projectId = null,
        string name = Constants.ProjectSprint.Name,
        DateOnly? startDate = null,
        DateOnly? endDate = null)
    {
        return new CreateProjectSprintRequest(
            projectId ?? Guid.NewGuid(),
            name,
            startDate,
            endDate);
    }

    public static UpdateProjectSprintRequest CreateUpdateProjectSprintRequest(
        Guid? id = null,
        string name = Constants.ProjectSprint.Name,
        DateOnly? startDate = null,
        DateOnly? endDate = null)
    {
        return new UpdateProjectSprintRequest(
            id ?? Guid.NewGuid(),
            name,
            startDate,
            endDate);
    }
    
    public static DeleteProjectSprintRequest CreateDeleteProjectSprintRequest(Guid? id = null)
    {
        return new DeleteProjectSprintRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }
}
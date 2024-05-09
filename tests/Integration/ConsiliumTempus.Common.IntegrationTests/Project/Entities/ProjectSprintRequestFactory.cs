using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Create;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Get;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.GetCollection;
using ConsiliumTempus.Common.IntegrationTests.TestConstants;

namespace ConsiliumTempus.Common.IntegrationTests.Project.Entities;

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
        Guid? projectId = null)
    {
        return new GetCollectionProjectSprintRequest
        {
            ProjectId = projectId ?? Guid.NewGuid()
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
}
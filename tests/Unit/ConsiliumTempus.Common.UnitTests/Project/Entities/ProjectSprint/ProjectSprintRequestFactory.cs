using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Create;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.GetCollection;
using ConsiliumTempus.Common.UnitTests.TestConstants;

namespace ConsiliumTempus.Common.UnitTests.Project.Entities.ProjectSprint;

public static class ProjectSprintRequestFactory
{
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
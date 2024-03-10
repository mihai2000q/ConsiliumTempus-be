using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Create;
using ConsiliumTempus.Api.IntegrationTests.TestConstants;

namespace ConsiliumTempus.Api.IntegrationTests.TestFactory;

public static class ProjectSprintRequestFactory
{
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
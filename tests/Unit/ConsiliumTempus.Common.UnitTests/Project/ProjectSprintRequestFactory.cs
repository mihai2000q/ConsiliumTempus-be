using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Create;
using ConsiliumTempus.Common.UnitTests.TestConstants;

namespace ConsiliumTempus.Common.UnitTests.Project;

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
using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.IntegrationTests.TestConstants;

namespace ConsiliumTempus.Api.IntegrationTests.TestFactory.Request;

public static class ProjectRequestFactory
{
    public static CreateProjectRequest CreateCreateProjectRequest(
        Guid? workspaceId = null,
        string name = Constants.Project.Name,
        string description = Constants.Project.Description,
        bool isPrivate = false)
    {
        return new CreateProjectRequest(
            workspaceId ?? Guid.NewGuid(),
            name,
            description,
            isPrivate);
    }
}
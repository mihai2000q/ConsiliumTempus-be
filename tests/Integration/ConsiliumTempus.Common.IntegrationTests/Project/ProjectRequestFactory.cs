using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.Contracts.Project.GetCollectionForWorkspace;
using ConsiliumTempus.Common.IntegrationTests.TestConstants;

namespace ConsiliumTempus.Common.IntegrationTests.Project;

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

    public static GetCollectionProjectForWorkspaceRequest CreateGetCollectionProjectForWorkspaceRequest(
        Guid? workspaceId = null)
    {
        return new GetCollectionProjectForWorkspaceRequest
        {
            WorkspaceId = workspaceId ?? Guid.NewGuid()
        };
    }
}
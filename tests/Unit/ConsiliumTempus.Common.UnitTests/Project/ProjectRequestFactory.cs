using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.Contracts.Project.Get;
using ConsiliumTempus.Api.Contracts.Project.GetCollectionForWorkspace;
using ConsiliumTempus.Common.UnitTests.TestConstants;

namespace ConsiliumTempus.Common.UnitTests.Project;

public static class ProjectRequestFactory
{
    public static GetProjectRequest CreateGetProjectRequest(
        Guid? id = null)
    {
        return new GetProjectRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }

    public static GetCollectionProjectForWorkspaceRequest CreateGetCollectionProjectForWorkspaceRequest(
        Guid? workspaceId = null)
    {
        return new GetCollectionProjectForWorkspaceRequest
        {
            WorkspaceId = workspaceId ?? Guid.NewGuid()
        };
    }

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
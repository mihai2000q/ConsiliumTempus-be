using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.Contracts.Workspace.Get;
using ConsiliumTempus.Api.Contracts.Workspace.Update;
using ConsiliumTempus.Api.IntegrationTests.TestConstants;

namespace ConsiliumTempus.Api.IntegrationTests.TestFactory;

public static class WorkspaceRequestFactory
{
    public static GetWorkspaceRequest CreateGetWorkspaceRequest(
        Guid? id = null)
    {
        return new GetWorkspaceRequest
        {
            Id = Guid.NewGuid()
        };
    }
    
    public static CreateWorkspaceRequest CreateCreateWorkspaceRequest(
        string name = Constants.Workspace.Name,
        string description = Constants.Workspace.Description)
    {
        return new CreateWorkspaceRequest(
            name,
            description);
    }
    
    public static UpdateWorkspaceRequest CreateUpdateWorkspaceRequest(
        Guid? id = null,
        string name = Constants.Workspace.Name,
        string description = Constants.Workspace.Description)
    {
        return new UpdateWorkspaceRequest(
            id ?? Guid.NewGuid(),
            name,
            description);
    }
}
using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Application.Workspace.Commands.Update;
using ConsiliumTempus.Common.UnitTests.TestConstants;

namespace ConsiliumTempus.Common.UnitTests.Workspace;

public static class WorkspaceCommandFactory
{
    public static CreateWorkspaceCommand CreateCreateWorkspaceCommand(
        string name = Constants.Workspace.Name)
    {
        return new CreateWorkspaceCommand(
            name);
    }

    public static UpdateWorkspaceCommand CreateUpdateWorkspaceCommand(
        Guid? id = null,
        string name = Constants.Workspace.Name,
        string description = Constants.Workspace.Description)
    {
        return new UpdateWorkspaceCommand(
            id ?? Guid.NewGuid(),
            name,
            description);
    }
}
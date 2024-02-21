using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Common.UnitTests.TestConstants;

namespace ConsiliumTempus.Common.UnitTests.Project;

public static class ProjectCommandFactory
{
    public static CreateProjectCommand CreateCreateProjectCommand(
        Guid? workspaceId = null,
        string name = Constants.Project.Name,
        string description = Constants.Project.Description,
        bool isPrivate = false,
        string token = Constants.Token)
    {
        return new CreateProjectCommand(
            workspaceId ?? Guid.NewGuid(), 
            name, 
            description, 
            isPrivate, 
            token);
    }
}
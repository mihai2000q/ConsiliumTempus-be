using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Application.Project.Commands.Update;
using ConsiliumTempus.Common.UnitTests.TestConstants;

namespace ConsiliumTempus.Common.UnitTests.Project;

public static class ProjectCommandFactory
{
    public static CreateProjectCommand CreateCreateProjectCommand(
        Guid? workspaceId = null,
        string name = Constants.Project.Name,
        bool isPrivate = false)
    {
        return new CreateProjectCommand(
            workspaceId ?? Guid.NewGuid(),
            name,
            isPrivate);
    }
    
    public static UpdateProjectCommand CreateUpdateProjectCommand(
        Guid? id = null,
        string name = Constants.Project.Name,
        bool isFavorite = false)
    {
        return new UpdateProjectCommand(
            id ?? Guid.NewGuid(),
            name,
            isFavorite);
    }
    
    public static DeleteProjectCommand CreateDeleteProjectCommand(Guid? id = null)
    {
        return new DeleteProjectCommand(id ?? Guid.NewGuid());
    }
}
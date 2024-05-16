using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Create;
using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Delete;
using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Update;
using ConsiliumTempus.Application.Project.Entities.Sprint.Queries.GetCollection;
using ConsiliumTempus.Domain.Project.Entities;

namespace ConsiliumTempus.Common.UnitTests.Project.Entities.Sprint;

public static class ProjectSprintResultFactory
{
    public static GetCollectionProjectSprintResult CreateGetCollectionProjectSprintResult(
        List<ProjectSprintAggregate>? sprints = null)
    {
        return new GetCollectionProjectSprintResult(
            sprints ?? ProjectSprintFactory.CreateList());
    }
    
    public static CreateProjectSprintResult CreateCreateProjectSprintResult()
    {
        return new CreateProjectSprintResult();
    }
    
    public static UpdateProjectSprintResult CreateUpdateProjectSprintResult()
    {
        return new UpdateProjectSprintResult();
    }
    
    public static DeleteProjectSprintResult CreateDeleteProjectSprintResult()
    {
        return new DeleteProjectSprintResult();
    }
}
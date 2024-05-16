using ConsiliumTempus.Application.ProjectSprint.Commands.Create;
using ConsiliumTempus.Application.ProjectSprint.Commands.Delete;
using ConsiliumTempus.Application.ProjectSprint.Commands.Update;
using ConsiliumTempus.Application.ProjectSprint.Queries.GetCollection;
using ConsiliumTempus.Domain.ProjectSprint;

namespace ConsiliumTempus.Common.UnitTests.ProjectSprint;

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
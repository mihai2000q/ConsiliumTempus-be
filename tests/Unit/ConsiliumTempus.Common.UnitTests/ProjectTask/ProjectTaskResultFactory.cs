using ConsiliumTempus.Application.ProjectTask.Commands.Create;
using ConsiliumTempus.Application.ProjectTask.Commands.Delete;
using ConsiliumTempus.Application.ProjectTask.Queries.GetCollection;
using ConsiliumTempus.Domain.ProjectTask;

namespace ConsiliumTempus.Common.UnitTests.ProjectTask;

public static class ProjectTaskResultFactory
{
    public static GetCollectionProjectTaskResult CreateGetCollectionProjectTaskResult(
        List<ProjectTaskAggregate>? tasks = null,
        int totalCount = 25)
    {
        return new GetCollectionProjectTaskResult(
            tasks ?? ProjectTaskFactory.CreateList(),
            totalCount);
    }
    
    public static CreateProjectTaskResult CreateCreateProjectTaskResult()
    {
        return new CreateProjectTaskResult();
    }
    
    public static DeleteProjectTaskResult CreateDeleteProjectTaskResult()
    {
        return new DeleteProjectTaskResult();
    }
}
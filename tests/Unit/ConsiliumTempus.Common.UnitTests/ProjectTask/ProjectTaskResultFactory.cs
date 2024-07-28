using ConsiliumTempus.Application.ProjectTask.Commands.Create;
using ConsiliumTempus.Application.ProjectTask.Commands.Delete;
using ConsiliumTempus.Application.ProjectTask.Commands.Move;
using ConsiliumTempus.Application.ProjectTask.Commands.Update;
using ConsiliumTempus.Application.ProjectTask.Commands.UpdateIsCompleted;
using ConsiliumTempus.Application.ProjectTask.Commands.UpdateOverview;
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

    public static MoveProjectTaskResult CreateMoveProjectTaskResult()
    {
        return new MoveProjectTaskResult();
    }

    public static UpdateProjectTaskResult CreateUpdateProjectTaskResult()
    {
        return new UpdateProjectTaskResult();
    }

    public static UpdateIsCompletedProjectTaskResult CreateUpdateIsCompletedProjectTaskResult()
    {
        return new UpdateIsCompletedProjectTaskResult();
    }

    public static UpdateOverviewProjectTaskResult CreateUpdateOverviewProjectTaskResult()
    {
        return new UpdateOverviewProjectTaskResult();
    }

    public static DeleteProjectTaskResult CreateDeleteProjectTaskResult()
    {
        return new DeleteProjectTaskResult();
    }
}
using ConsiliumTempus.Api.Contracts.ProjectTask.Create;
using ConsiliumTempus.Api.Contracts.ProjectTask.Delete;
using ConsiliumTempus.Api.Contracts.ProjectTask.Get;
using ConsiliumTempus.Api.Contracts.ProjectTask.GetCollection;
using ConsiliumTempus.Api.Contracts.ProjectTask.Move;
using ConsiliumTempus.Api.Contracts.ProjectTask.Update;
using ConsiliumTempus.Api.Contracts.ProjectTask.UpdateOverview;
using ConsiliumTempus.Common.UnitTests.TestConstants;

namespace ConsiliumTempus.Common.UnitTests.ProjectTask;

public static class ProjectTaskRequestFactory
{
    public static GetProjectTaskRequest CreateGetProjectTaskRequest(
        Guid? id = null)
    {
        return new GetProjectTaskRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }

    public static GetCollectionProjectTaskRequest CreateGetCollectionProjectTaskRequest(
        Guid? projectStageId = null,
        string[]? search = null,
        string[]? orderBy = null,
        int? currentPage = null,
        int? pageSize = null)
    {
        return new GetCollectionProjectTaskRequest
        {
            ProjectStageId = projectStageId ?? Guid.NewGuid(),
            Search = search,
            OrderBy = orderBy,
            CurrentPage = currentPage,
            PageSize = pageSize
        };
    }

    public static CreateProjectTaskRequest CreateCreateProjectTaskRequest(
        Guid? projectStageId = null,
        string name = Constants.ProjectTask.Name,
        bool onTop = false)
    {
        return new CreateProjectTaskRequest(
            projectStageId ?? Guid.NewGuid(),
            name,
            onTop);
    }

    public static UpdateProjectTaskRequest CreateUpdateProjectTaskRequest(
        Guid? id = null,
        string name = Constants.ProjectTask.Name,
        bool isCompleted = false,
        Guid? assigneeId = null)
    {
        return new UpdateProjectTaskRequest(
            id ?? Guid.NewGuid(),
            name,
            isCompleted,
            assigneeId);
    }

    public static UpdateOverviewProjectTaskRequest CreateUpdateOverviewProjectTaskRequest(
        Guid? id = null,
        string name = Constants.ProjectTask.Name,
        string description = Constants.ProjectTask.Description,
        bool isCompleted = false,
        Guid? assigneeId = null)
    {
        return new UpdateOverviewProjectTaskRequest(
            id ?? Guid.NewGuid(),
            name,
            description,
            isCompleted,
            assigneeId);
    }
    
    public static MoveProjectTaskRequest CreateMoveProjectTaskRequest(
        Guid? id = null,
        Guid? overId = null)
    {
        return new MoveProjectTaskRequest(
            id ?? Guid.NewGuid(),
            overId ?? Guid.NewGuid());
    }

    public static DeleteProjectTaskRequest CreateDeleteProjectTaskRequest(Guid? id = null)
    {
        return new DeleteProjectTaskRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }
}
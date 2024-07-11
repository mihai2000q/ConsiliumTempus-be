using ConsiliumTempus.Application.ProjectSprint.Commands.AddStage;
using ConsiliumTempus.Application.ProjectSprint.Commands.Create;
using ConsiliumTempus.Application.ProjectSprint.Commands.Delete;
using ConsiliumTempus.Application.ProjectSprint.Commands.MoveStage;
using ConsiliumTempus.Application.ProjectSprint.Commands.RemoveStage;
using ConsiliumTempus.Application.ProjectSprint.Commands.Update;
using ConsiliumTempus.Application.ProjectSprint.Commands.UpdateStage;
using ConsiliumTempus.Application.ProjectSprint.Queries.GetCollection;
using ConsiliumTempus.Application.ProjectSprint.Queries.GetStages;
using ConsiliumTempus.Common.UnitTests.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.Entities;

namespace ConsiliumTempus.Common.UnitTests.ProjectSprint;

public static class ProjectSprintResultFactory
{
    public static GetCollectionProjectSprintResult CreateGetCollectionProjectSprintResult(
        List<ProjectSprintAggregate>? sprints = null,
        int totalCount = 25)
    {
        return new GetCollectionProjectSprintResult(
            sprints ?? ProjectSprintFactory.CreateList(),
            totalCount);
    }
    
    public static GetStagesFromProjectSprintResult CreateGetStagesFromProjectSprintResult(
        List<ProjectStage>? stages = null)
    {
        return new GetStagesFromProjectSprintResult(
            stages ?? ProjectStageFactory.CreateList());
    }
    
    public static CreateProjectSprintResult CreateCreateProjectSprintResult()
    {
        return new CreateProjectSprintResult();
    }
    
    public static AddStageToProjectSprintResult CreateAddStageToProjectSprintResult()
    {
        return new AddStageToProjectSprintResult();
    }
    
    public static UpdateProjectSprintResult CreateUpdateProjectSprintResult()
    {
        return new UpdateProjectSprintResult();
    }
    
    public static UpdateStageFromProjectSprintResult CreateUpdateStageFromProjectSprintResult()
    {
        return new UpdateStageFromProjectSprintResult();
    }
    
    public static MoveStageFromProjectSprintResult CreateMoveStageFromProjectSprintResult()
    {
        return new MoveStageFromProjectSprintResult();
    }
    
    public static DeleteProjectSprintResult CreateDeleteProjectSprintResult()
    {
        return new DeleteProjectSprintResult();
    }
    
    public static RemoveStageFromProjectSprintResult CreateRemoveStageFromProjectSprintResult()
    {
        return new RemoveStageFromProjectSprintResult();
    }
}
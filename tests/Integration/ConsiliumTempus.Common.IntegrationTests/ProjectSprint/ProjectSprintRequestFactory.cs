using ConsiliumTempus.Api.Contracts.ProjectSprint.AddStage;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Create;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Delete;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Get;
using ConsiliumTempus.Api.Contracts.ProjectSprint.GetCollection;
using ConsiliumTempus.Api.Contracts.ProjectSprint.GetStages;
using ConsiliumTempus.Api.Contracts.ProjectSprint.RemoveStage;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Update;
using ConsiliumTempus.Api.Contracts.ProjectSprint.UpdateStage;
using ConsiliumTempus.Common.IntegrationTests.TestConstants;
using ConsiliumTempus.Domain.Project.Enums;

namespace ConsiliumTempus.Common.IntegrationTests.ProjectSprint;

public static class ProjectSprintRequestFactory
{
    public static GetProjectSprintRequest CreateGetProjectSprintRequest(Guid? id = null)
    {
        return new GetProjectSprintRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }

    public static GetCollectionProjectSprintRequest CreateGetCollectionProjectSprintRequest(
        Guid? id = null,
        string[]? search = null,
        bool fromThisYear = false)
    {
        return new GetCollectionProjectSprintRequest
        {
            ProjectId = id ?? Guid.NewGuid(),
            Search = search,
            FromThisYear = fromThisYear
        };
    }

    public static GetStagesFromProjectSprintRequest CreateGetStagesFromProjectSprintRequest(Guid? id = null)
    {
        return new GetStagesFromProjectSprintRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }

    public static CreateProjectSprintRequest CreateCreateProjectSprintRequest(
        Guid? projectId = null,
        string name = Constants.ProjectSprint.Name,
        DateOnly? startDate = null,
        DateOnly? endDate = null,
        bool keepPreviousStages = false,
        CreateProjectSprintRequest.CreateProjectStatus? projectStatus = null)
    {
        return new CreateProjectSprintRequest(
            projectId ?? Guid.NewGuid(),
            name,
            startDate,
            endDate,
            keepPreviousStages,
            projectStatus);
    }

    public static CreateProjectSprintRequest.CreateProjectStatus CreateCreateProjectStatus(
        string title = Constants.ProjectStatus.Title,
        string? status = null,
        string description = Constants.ProjectStatus.Description)
    {
        return new CreateProjectSprintRequest.CreateProjectStatus(
            title,
            status ?? ProjectStatusType.AtRisk.ToString(),
            description);
    }

    public static AddStageToProjectSprintRequest CreateAddStageToProjectSprintRequest(
        Guid? id = null,
        string name = Constants.ProjectStage.Name,
        bool onTop = false)
    {
        return new AddStageToProjectSprintRequest(
            id ?? Guid.NewGuid(),
            name,
            onTop);
    }

    public static UpdateProjectSprintRequest CreateUpdateProjectSprintRequest(
        Guid? id = null,
        string name = Constants.ProjectSprint.Name,
        DateOnly? startDate = null,
        DateOnly? endDate = null)
    {
        return new UpdateProjectSprintRequest(
            id ?? Guid.NewGuid(),
            name,
            startDate,
            endDate);
    }

    public static UpdateStageFromProjectSprintRequest CreateUpdateStageFromProjectSprintRequest(
        Guid? id = null,
        Guid? stageId = null,
        string name = Constants.ProjectStage.Name)
    {
        return new UpdateStageFromProjectSprintRequest(
            id ?? Guid.NewGuid(),
            stageId ?? Guid.NewGuid(),
            name);
    }

    public static DeleteProjectSprintRequest CreateDeleteProjectSprintRequest(Guid? id = null)
    {
        return new DeleteProjectSprintRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }

    public static RemoveStageFromProjectSprintRequest CreateRemoveStageFromProjectSprintRequest(
        Guid? id = null,
        Guid? stageId = null)
    {
        return new RemoveStageFromProjectSprintRequest
        {
            Id = id ?? Guid.NewGuid(),
            StageId = stageId ?? Guid.NewGuid()
        };
    }
}
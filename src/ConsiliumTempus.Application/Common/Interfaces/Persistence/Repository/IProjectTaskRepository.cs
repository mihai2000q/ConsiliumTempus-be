using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;

public interface IProjectTaskRepository
{
    Task<ProjectTaskAggregate?> GetWithWorkspace(
        ProjectTaskId id,
        CancellationToken cancellationToken = default);

    Task<ProjectTaskAggregate?> GetWithStagesAndWorkspace(
        ProjectTaskId id,
        CancellationToken cancellationToken = default);

    Task<ProjectTaskAggregate?> GetWithTasksAndWorkspace(
        ProjectTaskId id,
        CancellationToken cancellationToken = default);

    Task<ProjectTaskAggregate?> GetWithCustomFieldsAndWorkspace(
        ProjectTaskId id,
        CancellationToken cancellationToken = default);

    Task<List<ProjectTaskAggregate>> GetListByStage(
        ProjectStageId stageId,
        IReadOnlyList<IFilter<ProjectTaskAggregate>> filters,
        IReadOnlyList<IOrder<ProjectTaskAggregate>> orders,
        PaginationInfo? paginationInfo,
        CancellationToken cancellationToken = default);

    Task<int> GetListByStageCount(
        ProjectStageId stageId,
        IReadOnlyList<IFilter<ProjectTaskAggregate>> filters,
        CancellationToken cancellationToken = default);

    Task<List<ProjectTaskAggregate>> GetListByProject(
        ProjectId projectId,
        CancellationToken cancellationToken = default);

    Task DeleteCustomFieldsByTask(
        ProjectTaskId id,
        CancellationToken cancellationToken = default);

    Task DeleteCustomFieldsByProjectAndSetup(
        CustomFieldSetupAggregate customFieldSetup,
        ProjectAggregate project,
        CancellationToken cancellationToken = default);
}
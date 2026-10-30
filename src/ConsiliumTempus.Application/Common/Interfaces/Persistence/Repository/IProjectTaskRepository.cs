using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;

public interface IProjectTaskRepository
{
    Task<ProjectTaskAggregate?> GetWithWorkspace(
        ProjectTaskId id,
        CancellationToken cancellationToken = default);

    Task<ProjectTaskAggregate?> GetWithTasksAndWorkspace(
        ProjectTaskId id,
        CancellationToken cancellationToken = default);

    Task<ProjectTaskAggregate?> GetWithCustomFieldsAndWorkspace(
        ProjectTaskId id,
        CancellationToken cancellationToken = default);

    Task<ProjectTaskAggregate?> GetWithCustomFieldsStagesAndWorkspace(
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
    
    Task<List<MultiSelectCustomField>> GetMultiSelectCustomFieldsBySetup(
        MultiSelectCustomFieldSetupAggregate customFieldSetup,
        CancellationToken cancellationToken = default);

    Task<List<SingleSelectCustomField>> GetSingleSelectCustomFieldsBySetup(
        SingleSelectCustomFieldSetupAggregate customFieldSetup,
        CancellationToken cancellationToken = default);

    void MarkAsUpdated(ProjectTaskAggregate task);

    Task DeleteCustomFieldsByProjectAndSetup(
        CustomFieldSetupAggregate customFieldSetup,
        ProjectAggregate project,
        CancellationToken cancellationToken = default);
}
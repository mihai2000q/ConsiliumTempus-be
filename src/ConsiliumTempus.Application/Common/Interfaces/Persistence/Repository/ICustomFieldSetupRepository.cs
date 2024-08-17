using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;

public interface ICustomFieldSetupRepository
{
    Task<CustomFieldSetupAggregate?> Get(CustomFieldSetupId id, CancellationToken cancellationToken = default);

    Task<List<CustomFieldSetupAggregate>> GetList(
        WorkspaceId? workspaceId,
        ProjectId? projectId,
        CancellationToken cancellationToken = default);

    Task<List<CustomFieldSetupAggregate>> GetListByWorkspaceOrProjects(
        WorkspaceId workspaceId,
        List<ProjectAggregate> projects,
        CancellationToken cancellationToken = default);

    Task Add(CustomFieldSetupAggregate customFieldSetup, CancellationToken cancellationToken = default);

    void Remove(CustomFieldSetupAggregate customFieldSetup);

    void RemoveRange(List<CustomFieldSetupAggregate> customFieldSetup);
}
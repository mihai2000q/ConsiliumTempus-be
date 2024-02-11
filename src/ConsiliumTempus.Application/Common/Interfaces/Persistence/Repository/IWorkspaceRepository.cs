using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;

public interface IWorkspaceRepository
{
    Task<WorkspaceAggregate?> Get(WorkspaceId id, CancellationToken cancellationToken = default);
        
    Task Add(WorkspaceAggregate workspace, CancellationToken cancellationToken = default);

    void Remove(WorkspaceAggregate workspace);
}
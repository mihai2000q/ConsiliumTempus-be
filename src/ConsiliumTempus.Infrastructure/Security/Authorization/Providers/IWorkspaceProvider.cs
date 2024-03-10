using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Infrastructure.Security.Authorization.Providers;

public interface IWorkspaceProvider
{
    Task<WorkspaceAggregate?> Get(WorkspaceId id, CancellationToken cancellationToken = default);
    
    Task<WorkspaceAggregate?> GetByProject(ProjectId id, CancellationToken cancellationToken = default);
    
    Task<WorkspaceAggregate?> GetByProjectSprint(ProjectSprintId id, CancellationToken cancellationToken = default);
}
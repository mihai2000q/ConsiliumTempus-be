using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Infrastructure.Authorization.Providers;

public interface IWorkspaceProvider
{
    Task<WorkspaceAggregate?> Get(WorkspaceId id);
}
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Infrastructure.Authorization.Providers;
using ConsiliumTempus.Infrastructure.Persistence.Database;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public sealed class WorkspaceRepository(ConsiliumTempusDbContext dbContext) : IWorkspaceRepository, IWorkspaceProvider
{
    public async Task<WorkspaceAggregate?> Get(WorkspaceId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Workspaces.FindAsync([id], cancellationToken);
    }
    
    public async Task Add(WorkspaceAggregate workspaceAggregate, CancellationToken cancellationToken = default)
    {
        await dbContext.AddAsync(workspaceAggregate, cancellationToken);
    }

    public void Remove(WorkspaceAggregate workspace)
    {
        dbContext.Workspaces.Remove(workspace);
    }
}
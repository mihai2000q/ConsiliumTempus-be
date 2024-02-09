using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Infrastructure.Authorization.Providers;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public class WorkspaceRepository(ConsiliumTempusDbContext dbContext) : IWorkspaceRepository, IWorkspaceProvider
{
    public async Task<WorkspaceAggregate?> Get(WorkspaceId id)
    {
        return await dbContext.Workspaces.SingleOrDefaultAsync(w => w.Id == id);
    }
    
    public async Task Add(WorkspaceAggregate workspaceAggregate)
    {
        await dbContext.AddAsync(workspaceAggregate);
    }
}
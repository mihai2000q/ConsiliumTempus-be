using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Infrastructure.Persistence.Database;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public class WorkspaceRepository(ConsiliumTempusDbContext dbContext) : IWorkspaceRepository
{
    public async Task Add(WorkspaceAggregate workspaceAggregate)
    {
        await dbContext.AddAsync(workspaceAggregate);
        await dbContext.SaveChangesAsync();
    }
}
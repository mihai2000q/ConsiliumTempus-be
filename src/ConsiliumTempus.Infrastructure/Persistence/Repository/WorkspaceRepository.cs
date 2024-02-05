using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Domain.WorkspaceAggregate;
using ConsiliumTempus.Infrastructure.Persistence.Database;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public class WorkspaceRepository(ConsiliumTempusDbContext dbContext) : IWorkspaceRepository
{
    public async Task Add(Workspace workspace)
    {
        await dbContext.AddAsync(workspace);
        await dbContext.SaveChangesAsync();
    }
}
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Infrastructure.Persistence.Database;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public class WorkspaceRoleRepository(ConsiliumTempusDbContext dbContext) : IWorkspaceRoleRepository
{
    public void Attach(WorkspaceRole workspaceRole)
    {
        dbContext.Attach(workspaceRole);
    }
}
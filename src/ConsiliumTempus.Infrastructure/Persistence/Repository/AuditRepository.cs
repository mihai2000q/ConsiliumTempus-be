using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Infrastructure.Persistence.Database;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public sealed class AuditRepository(ConsiliumTempusDbContext dbContext) : IAuditRepository
{
    public void Remove(Audit audit)
    {
        dbContext.Remove(audit);
    }
}
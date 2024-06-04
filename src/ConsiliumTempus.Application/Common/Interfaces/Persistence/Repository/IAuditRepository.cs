using ConsiliumTempus.Domain.Common.Entities;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;

public interface IAuditRepository
{
    void Remove(Audit audit);
}
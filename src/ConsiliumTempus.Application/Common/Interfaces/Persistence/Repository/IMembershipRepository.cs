using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Relations;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;

public interface IMembershipRepository
{
    Task Add(Membership membership);
}
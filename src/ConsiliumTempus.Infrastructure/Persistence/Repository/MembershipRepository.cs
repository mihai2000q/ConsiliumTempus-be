using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Infrastructure.Persistence.Database;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public sealed class MembershipRepository(ConsiliumTempusDbContext dbContext) : IMembershipRepository
{
    public async Task Add(Membership membership)
    {
        await dbContext.AddAsync(membership);
    }
}
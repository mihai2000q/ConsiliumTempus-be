using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Relations;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public class MembershipRepository(ConsiliumTempusDbContext dbContext) : IMembershipRepository
{
    public async Task Add(Membership membership)
    {
        await dbContext.AddAsync(membership);
    }
}
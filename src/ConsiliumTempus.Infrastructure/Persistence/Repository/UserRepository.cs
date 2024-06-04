using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public sealed class UserRepository(ConsiliumTempusDbContext dbContext) : IUserRepository, IUserProvider
{
    public async Task<UserAggregate?> Get(UserId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.FindAsync([id], cancellationToken);
    }

    public async Task<UserAggregate?> GetByEmail(string email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.SingleOrDefaultAsync(
            u => u.Credentials.Email == email,
            cancellationToken);
    }

    public async Task Add(UserAggregate user, CancellationToken cancellationToken = default)
    {
        await dbContext.AddAsync(user, cancellationToken);
    }

    public void Remove(UserAggregate user)
    {
        dbContext.Remove(user);
    }

    public async Task NullifyAuditsByUser(UserAggregate user, CancellationToken cancellationToken = default)
    {
        var audits = await dbContext.Set<Audit>()
            .Where(a => a.UpdatedBy == user || a.CreatedBy == user)
            .ToListAsync(cancellationToken);

        audits.ForEach(a => a.Nullify());
    }

    public async Task RemoveProjectsByOwner(UserAggregate owner, CancellationToken cancellationToken = default)
    {
        await dbContext.Projects
            .Where(p => p.Owner == owner)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Infrastructure.Authorization.Providers;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public sealed class UserRepository(ConsiliumTempusDbContext dbContext) : IUserRepository, IUserProvider
{
    public async Task<UserAggregate?> Get(UserId id, CancellationToken cancellationToken = default)
    {   
        return await dbContext.Users.SingleOrDefaultAsync(
            u => u.Id == id, 
            cancellationToken);
    }

    public async Task<UserAggregate?> GetUserByEmail(string email, CancellationToken cancellationToken = default)
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
}
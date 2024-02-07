using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Infrastructure.Authorization;
using ConsiliumTempus.Infrastructure.Authorization.Providers;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public class UserRepository(ConsiliumTempusDbContext dbContext) : IUserRepository, IUserProvider
{
    public async Task<UserAggregate?> Get(UserId id)
    {   
        return await dbContext.Users.SingleOrDefaultAsync(u => u.Id == id);
    }

    public async Task<UserAggregate?> GetUserByEmail(string email)
    {
        return await dbContext.Users.SingleOrDefaultAsync(u => u.Credentials.Email == email);
    }

    public async Task Add(UserAggregate userAggregate)
    {
        await dbContext.AddAsync(userAggregate);
        await dbContext.SaveChangesAsync();
    }
}
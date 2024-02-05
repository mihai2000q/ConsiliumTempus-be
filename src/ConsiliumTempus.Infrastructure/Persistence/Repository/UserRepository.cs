using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public class UserRepository(ConsiliumTempusDbContext dbContext) : IUserRepository
{
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
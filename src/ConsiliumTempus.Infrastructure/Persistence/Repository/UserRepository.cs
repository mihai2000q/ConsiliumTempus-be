using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Domain.UserAggregate;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public class UserRepository(ConsiliumTempusDbContext dbContext) : IUserRepository
{
    public async Task<User?> GetUserByEmail(string email)
    {
        return await dbContext.Users.SingleOrDefaultAsync(u => u.Email == email);
    }

    public async Task Add(User user)
    {
        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync();
    }
}
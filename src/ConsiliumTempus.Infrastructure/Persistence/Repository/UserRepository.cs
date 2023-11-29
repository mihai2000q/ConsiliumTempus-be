using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Domain.UserAggregate;
using ConsiliumTempus.Infrastructure.Persistence.Database;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public class UserRepository : IUserRepository
{
    private readonly ConsiliumTempusDbContext _dbContext;

    public UserRepository(ConsiliumTempusDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public User? GetUserByEmail(string email)
    {
        return _dbContext.Users.SingleOrDefault(u => u.Email == email);
    }

    public void Add(User user)
    {
        _dbContext.Add(user);
        _dbContext.SaveChanges();
    }
}
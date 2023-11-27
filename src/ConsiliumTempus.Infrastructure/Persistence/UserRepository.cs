using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Domain.UserAggregate;

namespace ConsiliumTempus.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    public User? GetUserByEmail(string email)
    {
        return null;
    }

    public void Add(User user)
    {
        
    }
}
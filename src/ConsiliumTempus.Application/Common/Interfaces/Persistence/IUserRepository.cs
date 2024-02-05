using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
    Task<UserAggregate?> Get(UserId id);
    
    Task<UserAggregate?> GetUserByEmail(string email);

    Task Add(UserAggregate userAggregate);
}
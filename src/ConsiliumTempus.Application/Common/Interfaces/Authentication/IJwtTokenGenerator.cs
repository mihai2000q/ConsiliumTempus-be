using ConsiliumTempus.Domain.UserAggregate;

namespace ConsiliumTempus.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
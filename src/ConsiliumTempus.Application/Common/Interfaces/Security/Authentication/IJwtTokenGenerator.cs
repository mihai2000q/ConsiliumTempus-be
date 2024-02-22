using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Application.Common.Interfaces.Security.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(UserAggregate userAggregate);
}
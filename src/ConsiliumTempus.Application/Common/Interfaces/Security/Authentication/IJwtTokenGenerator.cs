using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Application.Common.Interfaces.Security.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(UserAggregate userAggregate);

    string GenerateToken(UserAggregate user, Guid jti);

    Guid GetJwtIdFromToken(string token);
}
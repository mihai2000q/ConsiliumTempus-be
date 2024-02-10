using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(UserAggregate userAggregate);
    
    public string GetUserIdFromToken(string plainToken);
}
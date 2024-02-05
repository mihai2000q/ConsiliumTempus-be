using ConsiliumTempus.Domain.User;
using ErrorOr;

namespace ConsiliumTempus.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(UserAggregate userAggregate);
    
    public ErrorOr<string> GetUserIdFromToken(string plainToken);
}
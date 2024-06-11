using ConsiliumTempus.Domain.Authentication;

namespace ConsiliumTempus.Application.Common.Interfaces.Security.Authentication;

public interface IJwtTokenValidator
{
    bool ValidateRefreshToken(RefreshToken refreshToken);

    Task<bool> ValidateAccessToken(string token);
}
using ConsiliumTempus.Domain.Common.Entities;

namespace ConsiliumTempus.Application.Common.Interfaces.Security.Authentication;

public interface IJwtTokenValidator
{
    bool ValidateRefreshToken(RefreshToken? refreshToken, string jwtId);

    Task<bool> ValidateAccessToken(string token);
}
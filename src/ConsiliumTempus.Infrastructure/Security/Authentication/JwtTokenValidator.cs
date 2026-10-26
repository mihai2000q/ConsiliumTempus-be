using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ConsiliumTempus.Application.Common.Interfaces.Security.Authentication;
using ConsiliumTempus.Domain.Authentication;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace ConsiliumTempus.Infrastructure.Security.Authentication;

internal sealed class JwtTokenValidator(
    IOptions<JwtSettings> jwtOptions,
    IUserProvider userProvider) : IJwtTokenValidator
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    public bool ValidateRefreshToken(RefreshToken refreshToken)
    {
        return DateTime.UtcNow <= refreshToken.ExpiryDateTime && !refreshToken.IsInvalidated.Value;
    }

    public async Task<bool> ValidateAccessToken(string token)
    {
        var jwtSecurityToken = ReadToken(token);
        if (jwtSecurityToken is null) return false;

        if (jwtSecurityToken.Issuer != _jwtSettings.Issuer ||
            !jwtSecurityToken.Audiences.SequenceEqual([_jwtSettings.Audience]) ||
            jwtSecurityToken.SignatureAlgorithm != SecurityAlgorithms.HmacSha256)
        {
            return false;
        }

        var claims = jwtSecurityToken.Claims.ToArray();
        var sub = GetClaim(claims, JwtRegisteredClaimNames.Sub);

        if (!Guid.TryParse(sub, out var userId)) return false;

        var user = await userProvider.Get(UserId.Create(userId));

        return user is not null &&
               AreClaimsValid(claims, user);
    }

    private static bool AreClaimsValid(Claim[] claims, UserAggregate user)
    {
        return GetClaim(claims, JwtRegisteredClaimNames.Email) == user.Credentials.Email &&
               Guid.TryParse(GetClaim(claims, JwtRegisteredClaimNames.Jti), out _);
    }

    private static JwtSecurityToken? ReadToken(string token)
    {
        try
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(token);
        }
        catch (Exception e) when (e is ArgumentException or SecurityTokenException)
        {
            return null;
        }
    }

    private static string? GetClaim(IEnumerable<Claim> claims, string type)
    {
        var matchingClaims = claims.Where(x => x.Type == type).Take(2).ToList();
        return matchingClaims.Count == 1 ? matchingClaims[0].Value : null;
    }
}
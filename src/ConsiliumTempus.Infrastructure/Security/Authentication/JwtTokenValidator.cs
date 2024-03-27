using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ConsiliumTempus.Application.Common.Interfaces.Security.Authentication;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace ConsiliumTempus.Infrastructure.Security.Authentication;

public sealed class JwtTokenValidator(IUserProvider userProvider) : IJwtTokenValidator
{
    public bool ValidateRefreshToken(RefreshToken? refreshToken, string jwtId)
    {
        return refreshToken is not null &&
               DateTime.UtcNow <= refreshToken.ExpiryDate &&
               !refreshToken.Invalidated &&
               refreshToken.JwtId.ToString() == jwtId;
    }
    
    public async Task<bool> ValidateAccessToken(string token)
    {
        var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        // also check validation parameters i.e., issuer, audience etc
        var claims = jwtSecurityToken.Claims.ToArray();
        var sub = GetClaim(claims, JwtRegisteredClaimNames.Sub);

        if (!Guid.TryParse(sub, out var userId)) return false;

        var user = await userProvider.Get(UserId.Create(userId));
        
        return user is not null && 
               AreClaimsValid(claims, user);
    }
    
    private static bool AreClaimsValid(Claim[] claims, UserAggregate user)
    {
        return GetClaim(claims, JwtRegisteredClaimNames.GivenName) == user.FirstName.Value &&
               GetClaim(claims, JwtRegisteredClaimNames.FamilyName) == user.LastName.Value &&
               GetClaim(claims, JwtRegisteredClaimNames.Email) == user.Credentials.Email &&
               GetClaim(claims, JwtRegisteredClaimNames.Jti)?.Length == 36;
    }
    
    private static string? GetClaim(IEnumerable<Claim> claims, string type)
    {
        return claims.SingleOrDefault(x => x.Type == type)?.Value;
    }
}
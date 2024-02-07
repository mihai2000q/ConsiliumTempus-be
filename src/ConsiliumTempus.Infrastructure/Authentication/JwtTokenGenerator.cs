using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ConsiliumTempus.Application.Common.Interfaces.Authentication;
using ConsiliumTempus.Domain.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ConsiliumTempus.Infrastructure.Authentication;

public class JwtTokenGenerator(IOptions<JwtSettings> jwtOptions) : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    public string GenerateToken(UserAggregate userAggregate)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userAggregate.Id.Value.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, userAggregate.Credentials.Email),
            new Claim(JwtRegisteredClaimNames.GivenName, userAggregate.Name.First),
            new Claim(JwtRegisteredClaimNames.FamilyName, userAggregate.Name.Last),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var securityToken = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            expires: DateTime.UtcNow.AddHours(_jwtSettings.ExpiryHours),
            claims: claims,
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    public string GetUserIdFromToken(string plainToken) =>
        GetClaimFromToken(plainToken, JwtRegisteredClaimNames.Sub);
    
    private static string GetClaimFromToken(string plainToken, string claimType)
    {
        var token = new JwtSecurityTokenHandler().ReadJwtToken(plainToken);
        var claim = token.Claims.Single(c => c.Type == claimType);
        return claim.Value;
    }
}
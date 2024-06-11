using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ConsiliumTempus.Application.Common.Interfaces.Security.Authentication;
using ConsiliumTempus.Domain.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ConsiliumTempus.Infrastructure.Security.Authentication;

public sealed class JwtTokenGenerator(IOptions<JwtSettings> jwtOptions) : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    public string GenerateToken(UserAggregate user)
    {
        return GenerateToken(user, Guid.NewGuid());
    }

    public string GenerateToken(UserAggregate user, Guid jti)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Credentials.Email),
            new Claim(JwtRegisteredClaimNames.Jti, jti.ToString())
        };

        var securityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            claims: claims,
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    public Guid GetJwtIdFromToken(string token)
    {
        return Guid.Parse(new JwtSecurityTokenHandler()
            .ReadJwtToken(token)
            .Claims
            .Single(c => c.Type == JwtRegisteredClaimNames.Jti)
            .Value);
    }
}
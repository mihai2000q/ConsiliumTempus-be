using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Infrastructure.Security.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ConsiliumTempus.Infrastructure.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class Token
    {
        internal static string GenerateToken(
            JwtSettings jwtSettings,
            UserAggregate? user = null,
            string? algorithm = null,
            string? issuer = null,
            string? audience = null,
            string? sub = null,
            string? email = null,
            string? jti = null,
            Claim[]? extraClaims = null,
            DateTime? expires = null)
        {
            user ??= UserFactory.Create();

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                algorithm ?? SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, sub ?? user.Id.Value.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email ?? user.Credentials.Email),
                new Claim(JwtRegisteredClaimNames.Jti, jti ?? Guid.NewGuid().ToString())
            }
                .Concat(extraClaims ?? [])
                .ToArray();

            var securityToken = new JwtSecurityToken(
                issuer ?? jwtSettings.Issuer,
                audience ?? jwtSettings.Audience,
                expires: expires ?? DateTime.UtcNow.AddMinutes(jwtSettings.ExpiryMinutes),
                claims: claims,
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        internal static TokenValidationParameters GetTokenValidationParameters(JwtSettings jwtSettings)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    [$"{JwtSettings.SectionName}:{nameof(JwtSettings.SecretKey)}"] = jwtSettings.SecretKey,
                    [$"{JwtSettings.SectionName}:{nameof(JwtSettings.Issuer)}"] = jwtSettings.Issuer,
                    [$"{JwtSettings.SectionName}:{nameof(JwtSettings.Audience)}"] = jwtSettings.Audience,
                    [$"{JwtSettings.SectionName}:{nameof(JwtSettings.ExpiryMinutes)}"] =
                        jwtSettings.ExpiryMinutes.ToString()
                })
                .Build();
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddInfrastructure(configuration);

            using var provider = services.BuildServiceProvider();
            return provider
                .GetRequiredService<IOptionsMonitor<JwtBearerOptions>>()
                .Get(JwtBearerDefaults.AuthenticationScheme)
                .TokenValidationParameters;
        }
    }
}
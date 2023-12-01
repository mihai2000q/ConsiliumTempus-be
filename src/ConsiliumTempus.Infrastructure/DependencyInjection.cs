using System.Text;
using ConsiliumTempus.Application.Common.Interfaces.Authentication;
using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Infrastructure.Authentication;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using ConsiliumTempus.Infrastructure.Persistence.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ConsiliumTempus.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuth(configuration)
            .AddPersistence(configuration);
        
        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);
        
        services.AddSingleton(Options.Create(jwtSettings));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        
        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
            });
        
        services.AddSingleton<IScrambler, Scrambler>();
        
        return services;
    }
    
    private static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseSettings = new DatabaseSettings();
        configuration.Bind(DatabaseSettings.SectionName, databaseSettings);
        
        services.AddDbContext<ConsiliumTempusDbContext>(options => options
            .UseSqlServer($"" +
                          $"Server={databaseSettings.Server},{databaseSettings.Port};" +
                          $"Database={databaseSettings.Name};" +
                          $"User Id={databaseSettings.User};" +
                          $"Password={databaseSettings.Password};" +
                          $"Encrypt=false")); 
        
        services.AddScoped<IUserRepository, UserRepository>();
    }
}
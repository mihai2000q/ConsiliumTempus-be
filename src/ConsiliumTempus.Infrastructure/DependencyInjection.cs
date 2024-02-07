using System.Text;
using ConsiliumTempus.Application.Common.Interfaces.Authentication;
using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Infrastructure.Authentication;
using ConsiliumTempus.Infrastructure.Authorization;
using ConsiliumTempus.Infrastructure.Authorization.Token;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using ConsiliumTempus.Infrastructure.Persistence.Interceptors;
using ConsiliumTempus.Infrastructure.Persistence.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        services.AddAppAuthentication(configuration)
            .AddAppAuthorization()
            .AddPersistence(configuration);

        return services;
    }

    private static IServiceCollection AddAppAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));

        services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true
                };
                options.MapInboundClaims = false;
            });
        
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IScrambler, Scrambler>();

        return services;
    }

    private static IServiceCollection AddAppAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization();

        services.AddSingleton<IAuthorizationHandler, TokenAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
        
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

        services.AddInterceptors()
            .AddRepositories();
    }

    private static IServiceCollection AddInterceptors(this IServiceCollection services)
    {
        services.AddScoped<PublishDomainEventInterceptor>();

        return services;
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
    }
}
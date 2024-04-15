using System.Reflection;
using ConsiliumTempus.Api.Common.Cors;
using ConsiliumTempus.Api.Common.Errors;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Serilog;

namespace ConsiliumTempus.Api;

public static class DependencyInjection
{
    public static void AddPresentation(this IServiceCollection services)
    {
        services.AddHttpContextAccessor()
            .AddMappings()
            .AddSingleton<ProblemDetailsFactory, ConsiliumTempusProblemDetailsFactory>()
            .AddCorsPolicies()
            .AddControllers();
    }

    public static void AddLogger(this ILoggingBuilder logging, IConfiguration configuration)
    {
        logging.ClearProviders();

        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

        logging.AddSerilog(logger);
    }

    private static IServiceCollection AddMappings(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
        return services;
    }

    private static IServiceCollection AddCorsPolicies(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicies.Frontend.Policy, policy =>
            {
                policy
                    .WithOrigins(CorsPolicies.Frontend.Origin)
                    .WithMethods(CorsPolicies.Frontend.Methods)
                    //.WithHeaders(CorsPolicies.Frontend.Headers)
                    .AllowAnyHeader();
            });
        });
        return services;
    }
}
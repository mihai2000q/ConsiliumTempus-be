using System.Reflection;
using System.Text.Json.Serialization;
using ConsiliumTempus.Api.Common.Cors;
using ConsiliumTempus.Api.Common.Errors;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Serilog;

namespace ConsiliumTempus.Api;

internal static class DependencyInjection
{
    public static void AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor()
            .AddMappings()
            .AddSingleton<ProblemDetailsFactory, ConsiliumTempusProblemDetailsFactory>()
            .AddCorsPolicies(configuration)
            .AddControllers()
            .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
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

    extension(IServiceCollection services)
    {
        private IServiceCollection AddMappings()
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
            return services;
        }

        private IServiceCollection AddCorsPolicies(IConfiguration configuration)
        {
            var corsSettings = new CorsSettings();
            configuration.Bind(CorsSettings.SectionName, corsSettings);
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicies.Policy, policy =>
                {
                    policy
                        .WithOrigins(corsSettings.FrontendOrigin)
                        .WithMethods(CorsPolicies.Methods)
                        .AllowAnyHeader();
                });
            });
            return services;
        }
    }
}
using ConsiliumTempus.Api.Common;
using ConsiliumTempus.Api.Common.Errors;
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
}
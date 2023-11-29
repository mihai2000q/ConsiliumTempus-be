using ConsiliumTempus.Api.Common;
using ConsiliumTempus.Api.Common.Errors;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ConsiliumTempus.Api;

public static class DependencyInjection
{
    public static void AddPresentation(this IServiceCollection services)
    {
        services.AddMappings()
            .AddSingleton<ProblemDetailsFactory, ConsiliumTempusProblemDetailsFactory>()
            .AddControllers();
    }
}
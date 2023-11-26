using System.Reflection;
using ConsiliumTempus.Application.Common.Behavior;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ConsiliumTempus.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        
        
        return services;
    }
}
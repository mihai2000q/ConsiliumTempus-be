using System.Reflection;
using ConsiliumTempus.Application.Common.Behaviors;
using ConsiliumTempus.Application.Common.Security;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ConsiliumTempus.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
        });
        
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<ISecurity, Security>();

        return services;
    }
}
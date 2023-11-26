﻿using System.Reflection;
using Mapster;
using MapsterMapper;

namespace ConsiliumTempus.Api.Common;

public static class MappingDependencyInjection
{
    public static IServiceCollection AddMappings(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
        return services;
    }
}
using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Api.Contracts.Authentication.Login;
using ConsiliumTempus.Api.Contracts.Authentication.Refresh;
using ConsiliumTempus.Api.Contracts.Authentication.Register;
using ConsiliumTempus.Application.Authentication.Commands.Login;
using ConsiliumTempus.Application.Authentication.Commands.Refresh;
using ConsiliumTempus.Application.Authentication.Commands.Register;
using Mapster;

namespace ConsiliumTempus.Api.Common.Mapping;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        RegisterMappings(config);
        LoginMappings(config);
    }

    private static void RefreshTokenMappings(TypeAdapterConfig config)
    {
        config.NewConfig<RefreshRequest, RefreshCommand>();
        
        config.NewConfig<RefreshResult, RefreshResponse>();
    }

    private static void RegisterMappings(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, RegisterCommand>();

        config.NewConfig<RegisterResult, RegisterResponse>();
    }

    private static void LoginMappings(TypeAdapterConfig config)
    {
        config.NewConfig<LoginRequest, LoginCommand>();

        config.NewConfig<LoginResult, LoginResponse>();
    }
}
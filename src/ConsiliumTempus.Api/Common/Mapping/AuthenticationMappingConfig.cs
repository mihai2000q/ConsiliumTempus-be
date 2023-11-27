using ConsiliumTempus.Api.Contracts.Authentication;
using ConsiliumTempus.Api.Contracts.Authentication.Login;
using ConsiliumTempus.Api.Contracts.Authentication.Register;
using ConsiliumTempus.Application.Authentication.Commands.Register;
using ConsiliumTempus.Application.Authentication.Queries;
using Mapster;

namespace ConsiliumTempus.Api.Common.Mapping;

public sealed class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        RegisterMappings(config);
        LoginMappings(config);
    }

    private static void RegisterMappings(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, RegisterCommand>();

        config.NewConfig<RegisterResult, RegisterResponse>();
    }

    private static void LoginMappings(TypeAdapterConfig config)
    {
        config.NewConfig<LoginRequest, LoginQuery>();
        
        config.NewConfig<LoginResult, LoginResponse>();
    }
}
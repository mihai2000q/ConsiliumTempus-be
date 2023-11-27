using ConsiliumTempus.Api.Contracts.Authentication;
using ConsiliumTempus.Api.Contracts.Authentication.Register;
using ConsiliumTempus.Application.Authentication.Commands.Register;
using Mapster;

namespace ConsiliumTempus.Api.Common.Mapping;

public sealed class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        RegisterMappings(config);
    }

    private static void RegisterMappings(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, RegisterCommand>();

        config.NewConfig<RegisterResult, RegisterResponse>();
    }
}
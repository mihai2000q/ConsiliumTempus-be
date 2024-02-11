using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Api.Contracts.User.Update;
using ConsiliumTempus.Api.Dto;
using ConsiliumTempus.Application.User.Commands.Update;
using Mapster;

namespace ConsiliumTempus.Api.Common.Mapping;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public sealed class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        PutMappings(config);
    }

    private static void PutMappings(TypeAdapterConfig config)
    {
        config.NewConfig<UpdateUserRequest, UpdateUserCommand>();
        
        config.NewConfig<UpdateUserResult, UserDto>()
            .Map(dest => dest.Id, src => src.User.Id.Value.ToString())
            .Map(dest => dest.FirstName, src => src.User.Name.First)
            .Map(dest => dest.LastName, src => src.User.Name.Last)
            .Map(dest => dest.Email, src => src.User.Credentials.Email);
    }
}
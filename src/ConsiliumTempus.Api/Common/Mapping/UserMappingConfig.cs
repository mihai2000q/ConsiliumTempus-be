using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Api.Contracts.User.Delete;
using ConsiliumTempus.Api.Contracts.User.Get;
using ConsiliumTempus.Api.Contracts.User.GetId;
using ConsiliumTempus.Api.Contracts.User.Update;
using ConsiliumTempus.Api.Dto;
using ConsiliumTempus.Application.User.Commands.Delete;
using ConsiliumTempus.Application.User.Commands.Update;
using ConsiliumTempus.Application.User.Queries.Get;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;
using Mapster;

namespace ConsiliumTempus.Api.Common.Mapping;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        GetMappings(config);
        PutMappings(config);
        DeleteMappings(config);
    }

    private static void GetMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetUserRequest, GetUserQuery>();
        
        config.NewConfig<UserAggregate, UserDto>()
            .IgnoreNullValues(true)
            .Map(dest => dest.Id, src => src.Id.Value.ToString())
            .Map(dest => dest.FirstName, src => src.FirstName.Value)
            .Map(dest => dest.LastName, src => src.LastName.Value)
            .Map(dest => dest.Email, src => src.Credentials.Email)
            .Map(dest => dest.Role, src => src.Role!.Value);

        config.NewConfig<UserId, GetUserIdResponse>()
            .Map(dest => dest.Id, src => src.ToString());
    }

    private static void PutMappings(TypeAdapterConfig config)
    {
        config.NewConfig<UpdateUserRequest, UpdateUserCommand>();
        
        config.NewConfig<UpdateUserResult, UserDto>()
            .IgnoreNullValues(true)
            .Map(dest => dest, src => src.User)
            .Map(dest => dest.Id, src => src.User.Id.Value.ToString())
            .Map(dest => dest.FirstName, src => src.User.FirstName.Value)
            .Map(dest => dest.LastName, src => src.User.LastName.Value)
            .Map(dest => dest.Email, src => src.User.Credentials.Email)
            .Map(dest => dest.Role, src => src.User.Role!.Value);
    }
    
    private static void DeleteMappings(TypeAdapterConfig config)
    {
        config.NewConfig<DeleteUserResult, DeleteUserResponse>();
    }
}
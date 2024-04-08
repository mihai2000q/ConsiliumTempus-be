using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Api.Contracts.User.DeleteCurrent;
using ConsiliumTempus.Api.Contracts.User.Get;
using ConsiliumTempus.Api.Contracts.User.GetCurrent;
using ConsiliumTempus.Api.Contracts.User.UpdateCurrent;
using ConsiliumTempus.Application.User.Commands.DeleteCurrent;
using ConsiliumTempus.Application.User.Commands.UpdateCurrent;
using ConsiliumTempus.Application.User.Queries.Get;
using ConsiliumTempus.Domain.User;
using Mapster;

namespace ConsiliumTempus.Api.Common.Mapping;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        GetMappings(config);
        GetCurrentMappings(config);
        UpdateCurrentMappings(config);
        DeleteMappings(config);
    }

    private static void GetMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetUserRequest, GetUserQuery>();
        
        config.NewConfig<UserAggregate, GetUserResponse>()
            .IgnoreNullValues(true)
            .Map(dest => dest.FirstName, src => src.FirstName.Value)
            .Map(dest => dest.LastName, src => src.LastName.Value)
            .Map(dest => dest.Email, src => src.Credentials.Email)
            .Map(dest => dest.Role, src => src.Role!.Value);
    }
    
    private static void GetCurrentMappings(TypeAdapterConfig config)
    {
        config.NewConfig<UserAggregate, GetCurrentUserResponse>()
            .IgnoreNullValues(true)
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.FirstName, src => src.FirstName.Value)
            .Map(dest => dest.LastName, src => src.LastName.Value)
            .Map(dest => dest.Email, src => src.Credentials.Email)
            .Map(dest => dest.Role, src => src.Role!.Value);
    }

    private static void UpdateCurrentMappings(TypeAdapterConfig config)
    {
        config.NewConfig<UpdateCurrentUserRequest, UpdateCurrentUserCommand>();

        config.NewConfig<UpdateCurrentUserResult, UpdateCurrentUserResponse>();
    }
    
    private static void DeleteMappings(TypeAdapterConfig config)
    {
        config.NewConfig<DeleteCurrentUserResult, DeleteCurrentUserResponse>();
    }
}
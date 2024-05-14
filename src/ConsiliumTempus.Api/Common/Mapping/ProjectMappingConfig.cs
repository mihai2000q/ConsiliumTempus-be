using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.Contracts.Project.Delete;
using ConsiliumTempus.Api.Contracts.Project.Get;
using ConsiliumTempus.Api.Contracts.Project.GetCollection;
using ConsiliumTempus.Api.Contracts.Project.GetCollectionForUser;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Application.Project.Queries.Get;
using ConsiliumTempus.Application.Project.Queries.GetCollection;
using ConsiliumTempus.Application.Project.Queries.GetCollectionForUser;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.Entities;
using Mapster;

namespace ConsiliumTempus.Api.Common.Mapping;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class ProjectMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        GetMappings(config);
        GetCollectionForUserMappings(config);
        GetCollectionMappings(config);
        CreateMappings(config);
        DeleteMappings(config);
    }

    private static void GetMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetProjectRequest, GetProjectQuery>();

        config.NewConfig<ProjectAggregate, GetProjectResponse>()
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description.Value)
            .Map(dest => dest.IsFavorite, src => src.IsFavorite.Value)
            .Map(dest => dest.IsPrivate, src => src.IsPrivate.Value);
        config.NewConfig<ProjectSprint, GetProjectResponse.ProjectSprintResponse>()
            .IgnoreNullValues(true)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Id, src => src.Id.Value);
    }

    private static void GetCollectionForUserMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetCollectionProjectForUserResult, GetCollectionProjectForUserResponse>();
        config.NewConfig<ProjectAggregate, GetCollectionProjectForUserResponse.ProjectResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value);
    }

    private static void GetCollectionMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetCollectionProjectRequest, GetCollectionProjectQuery>();

        config.NewConfig<GetCollectionProjectResult, GetCollectionProjectResponse>();
        config.NewConfig<ProjectAggregate, GetCollectionProjectResponse.ProjectResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description.Value)
            .Map(dest => dest.IsFavorite, src => src.IsFavorite.Value)
            .Map(dest => dest.IsPrivate, src => src.IsPrivate.Value);
    }

    private static void CreateMappings(TypeAdapterConfig config)
    {
        config.NewConfig<CreateProjectRequest, CreateProjectCommand>();

        config.NewConfig<CreateProjectResult, CreateProjectResponse>();
    }

    private static void DeleteMappings(TypeAdapterConfig config)
    {
        config.NewConfig<DeleteProjectResult, DeleteProjectResponse>();
    }
}
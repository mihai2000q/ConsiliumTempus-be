using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.Contracts.Project.Delete;
using ConsiliumTempus.Api.Contracts.Project.Get;
using ConsiliumTempus.Api.Contracts.Project.GetCollection;
using ConsiliumTempus.Api.Contracts.Project.GetOverview;
using ConsiliumTempus.Api.Contracts.Project.Update;
using ConsiliumTempus.Api.Contracts.Project.UpdateOverview;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Application.Project.Commands.Update;
using ConsiliumTempus.Application.Project.Commands.UpdateOverview;
using ConsiliumTempus.Application.Project.Queries.Get;
using ConsiliumTempus.Application.Project.Queries.GetCollection;
using ConsiliumTempus.Application.Project.Queries.GetOverview;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectSprint;
using Mapster;

namespace ConsiliumTempus.Api.Common.Mapping;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class ProjectMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        GetMappings(config);
        GetOverviewMappings(config);
        GetCollectionMappings(config);
        CreateMappings(config);
        UpdateMappings(config);
        UpdateOverviewMappings(config);
        DeleteMappings(config);
    }

    private static void GetMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetProjectRequest, GetProjectQuery>();

        config.NewConfig<ProjectAggregate, GetProjectResponse>()
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.IsFavorite, src => src.IsFavorite.Value)
            .Map(dest => dest.IsPrivate, src => src.IsPrivate.Value);
    }

    private static void GetOverviewMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetOverviewProjectRequest, GetOverviewProjectQuery>();

        config.NewConfig<GetOverviewProjectResult, GetOverviewProjectResponse>()
            .Map(dest => dest.Description, src => src.Description.Value);
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

    private static void UpdateMappings(TypeAdapterConfig config)
    {
        config.NewConfig<UpdateProjectRequest, UpdateProjectCommand>();

        config.NewConfig<UpdateProjectResult, UpdateProjectResponse>();
    }

    private static void UpdateOverviewMappings(TypeAdapterConfig config)
    {
        config.NewConfig<UpdateOverviewProjectRequest, UpdateOverviewProjectCommand>();

        config.NewConfig<UpdateOverviewProjectResult, UpdateOverviewProjectResponse>();
    }

    private static void DeleteMappings(TypeAdapterConfig config)
    {
        config.NewConfig<DeleteProjectRequest, DeleteProjectCommand>();

        config.NewConfig<DeleteProjectResult, DeleteProjectResponse>();
    }
}
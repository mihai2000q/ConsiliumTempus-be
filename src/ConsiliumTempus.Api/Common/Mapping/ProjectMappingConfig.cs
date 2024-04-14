using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.Contracts.Project.Delete;
using ConsiliumTempus.Api.Contracts.Project.Get;
using ConsiliumTempus.Api.Contracts.Project.GetCollectionForUser;
using ConsiliumTempus.Api.Contracts.Project.GetCollectionForWorkspace;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Application.Project.Queries.Get;
using ConsiliumTempus.Application.Project.Queries.GetCollectionForUser;
using ConsiliumTempus.Application.Project.Queries.GetCollectionForWorkspace;
using ConsiliumTempus.Domain.Project;
using Mapster;

namespace ConsiliumTempus.Api.Common.Mapping;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class ProjectMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        GetMappings(config);
        GetCollectionForUserMappings(config);
        GetCollectionForWorkspaceMappings(config);
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
    }

    private static void GetCollectionForUserMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetCollectionProjectForUserResult, GetCollectionProjectForUserResponse>();
        config.NewConfig<ProjectAggregate, GetCollectionProjectForUserResponse.ProjectResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value);
    }

    private static void GetCollectionForWorkspaceMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetCollectionProjectForWorkspaceRequest, GetCollectionProjectForWorkspaceQuery>();

        config.NewConfig<GetCollectionProjectForWorkspaceResult, GetCollectionProjectForWorkspaceResponse>();
        config.NewConfig<ProjectAggregate, GetCollectionProjectForWorkspaceResponse.ProjectResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description.Value);
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
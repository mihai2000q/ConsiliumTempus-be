using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Create;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Delete;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Get;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.GetCollection;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Update;
using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Create;
using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Delete;
using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Update;
using ConsiliumTempus.Application.Project.Entities.Sprint.Queries.Get;
using ConsiliumTempus.Application.Project.Entities.Sprint.Queries.GetCollection;
using ConsiliumTempus.Domain.Project.Entities;
using Mapster;

namespace ConsiliumTempus.Api.Common.Mapping;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class ProjectSprintMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        Get(config);
        GetCollectionMappings(config);
        CreateMappings(config);
        UpdateMappings(config);
        DeleteMappings(config);
    }
    
    private static void Get(TypeAdapterConfig config)
    {
        config.NewConfig<GetProjectSprintRequest, GetProjectSprintQuery>();

        config.NewConfig<ProjectSprint, GetProjectSprintResponse>()
            .Map(dest => dest.Name, src => src.Name.Value);
    }

    private static void GetCollectionMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetCollectionProjectSprintRequest, GetCollectionProjectSprintQuery>();

        config.NewConfig<GetCollectionProjectSprintResult, GetCollectionProjectSprintResponse>();
        config.NewConfig<ProjectSprint, GetCollectionProjectSprintResponse.ProjectSprintResponse>()
            .IgnoreNullValues(true)
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value);
    }

    private static void CreateMappings(TypeAdapterConfig config)
    {
        config.NewConfig<CreateProjectSprintRequest, CreateProjectSprintCommand>();

        config.NewConfig<CreateProjectSprintResult, CreateProjectSprintResponse>();
    }
    
    private static void UpdateMappings(TypeAdapterConfig config)
    {
        config.NewConfig<UpdateProjectSprintRequest, UpdateProjectSprintCommand>();

        config.NewConfig<UpdateProjectSprintResult, UpdateProjectSprintResponse>();
    }

    private static void DeleteMappings(TypeAdapterConfig config)
    {
        config.NewConfig<DeleteProjectSprintResult, DeleteProjectSprintResponse>();
    }
}
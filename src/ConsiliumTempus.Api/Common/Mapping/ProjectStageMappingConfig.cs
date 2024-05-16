using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Api.Contracts.Project.Entities.Stage.Create;
using ConsiliumTempus.Api.Contracts.Project.Entities.Stage.Delete;
using ConsiliumTempus.Api.Contracts.Project.Entities.Stage.GetCollection;
using ConsiliumTempus.Api.Contracts.Project.Entities.Stage.Update;
using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Create;
using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Delete;
using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Update;
using ConsiliumTempus.Application.Project.Entities.Stage.Queries.GetCollection;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using Mapster;

namespace ConsiliumTempus.Api.Common.Mapping;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class ProjectStageMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        GetCollection(config);
        CreateMappings(config);
        UpdateMappings(config);
        DeleteMappings(config);
    }
    
    private static void GetCollection(TypeAdapterConfig config)
    {
        config.NewConfig<GetCollectionProjectStageRequest, GetCollectionProjectStageQuery>();

        config.NewConfig<GetCollectionProjectStageResult, GetCollectionProjectStageResponse>();
        config.NewConfig<ProjectStage, GetCollectionProjectStageResponse.ProjectStageResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value);
    }

    private static void CreateMappings(TypeAdapterConfig config)
    {
        config.NewConfig<CreateProjectStageRequest, CreateProjectStageCommand>();

        config.NewConfig<CreateProjectStageResult, CreateProjectStageResponse>();
    }

    private static void UpdateMappings(TypeAdapterConfig config)
    {
        config.NewConfig<UpdateProjectStageRequest, UpdateProjectStageCommand>();

        config.NewConfig<UpdateProjectStageResult, UpdateProjectStageResponse>();
    }

    private static void DeleteMappings(TypeAdapterConfig config)
    {
        config.NewConfig<DeleteProjectStageRequest, DeleteProjectStageCommand>();

        config.NewConfig<DeleteProjectStageResult, DeleteProjectStageResponse>();
    }
}
using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Api.Contracts.ProjectSprint.AddStage;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Create;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Delete;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Get;
using ConsiliumTempus.Api.Contracts.ProjectSprint.GetCollection;
using ConsiliumTempus.Api.Contracts.ProjectSprint.RemoveStage;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Update;
using ConsiliumTempus.Api.Contracts.ProjectSprint.UpdateStage;
using ConsiliumTempus.Application.ProjectSprint.Commands.AddStage;
using ConsiliumTempus.Application.ProjectSprint.Commands.Create;
using ConsiliumTempus.Application.ProjectSprint.Commands.Delete;
using ConsiliumTempus.Application.ProjectSprint.Commands.RemoveStage;
using ConsiliumTempus.Application.ProjectSprint.Commands.Update;
using ConsiliumTempus.Application.ProjectSprint.Commands.UpdateStage;
using ConsiliumTempus.Application.ProjectSprint.Queries.Get;
using ConsiliumTempus.Application.ProjectSprint.Queries.GetCollection;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.User;
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
        AddStageMappings(config);
        UpdateMappings(config);
        UpdateStageMappings(config);
        DeleteMappings(config);
        RemoveStageMappings(config);
    }

    private static void Get(TypeAdapterConfig config)
    {
        config.NewConfig<GetProjectSprintRequest, GetProjectSprintQuery>();

        config.NewConfig<ProjectSprintAggregate, GetProjectSprintResponse>()
            .IgnoreNullValues(true)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.CreatedBy, src => src.Audit.CreatedBy)
            .Map(dest => dest.CreatedDateTime, src => src.Audit.CreatedDateTime)
            .Map(dest => dest.UpdatedBy, src => src.Audit.UpdatedBy)
            .Map(dest => dest.UpdatedDateTime, src => src.Audit.UpdatedDateTime);
        config.NewConfig<ProjectStage, GetProjectSprintResponse.ProjectStageResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value);
        config.NewConfig<UserAggregate, GetProjectSprintResponse.UserResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.FirstName.Value + " " + src.LastName.Value)
            .Map(dest => dest.Email, src => src.Credentials.Email);
    }

    private static void GetCollectionMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetCollectionProjectSprintRequest, GetCollectionProjectSprintQuery>();

        config.NewConfig<GetCollectionProjectSprintResult, GetCollectionProjectSprintResponse>();
        config.NewConfig<ProjectSprintAggregate, GetCollectionProjectSprintResponse.ProjectSprintResponse>()
            .IgnoreNullValues(true)
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.CreatedDateTime, src => src.Audit.CreatedDateTime);
    }

    private static void CreateMappings(TypeAdapterConfig config)
    {
        config.NewConfig<CreateProjectSprintRequest, CreateProjectSprintCommand>();

        config.NewConfig<CreateProjectSprintResult, CreateProjectSprintResponse>();
    }
    
    private static void AddStageMappings(TypeAdapterConfig config)
    {
        config.NewConfig<AddStageToProjectSprintRequest, AddStageToProjectSprintCommand>();

        config.NewConfig<AddStageToProjectSprintResult, AddStageToProjectSprintResponse>();
    }

    private static void UpdateMappings(TypeAdapterConfig config)
    {
        config.NewConfig<UpdateProjectSprintRequest, UpdateProjectSprintCommand>();

        config.NewConfig<UpdateProjectSprintResult, UpdateProjectSprintResponse>();
    }
    
    private static void UpdateStageMappings(TypeAdapterConfig config)
    {
        config.NewConfig<UpdateStageFromProjectSprintRequest, UpdateStageFromProjectSprintCommand>();

        config.NewConfig<UpdateStageFromProjectSprintResult, UpdateStageFromProjectSprintResponse>();
    }

    private static void DeleteMappings(TypeAdapterConfig config)
    {
        config.NewConfig<DeleteProjectSprintRequest, DeleteProjectSprintCommand>();

        config.NewConfig<DeleteProjectSprintResult, DeleteProjectSprintResponse>();
    }
    
    private static void RemoveStageMappings(TypeAdapterConfig config)
    {
        config.NewConfig<RemoveStageFromProjectSprintRequest, RemoveStageFromProjectSprintCommand>();

        config.NewConfig<RemoveStageFromProjectSprintResult, RemoveStageFromProjectSprintResponse>();
    }
}
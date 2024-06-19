using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Api.Contracts.ProjectTask.Create;
using ConsiliumTempus.Api.Contracts.ProjectTask.Delete;
using ConsiliumTempus.Api.Contracts.ProjectTask.Get;
using ConsiliumTempus.Api.Contracts.ProjectTask.GetCollection;
using ConsiliumTempus.Api.Contracts.ProjectTask.Update;
using ConsiliumTempus.Api.Contracts.ProjectTask.UpdateOverview;
using ConsiliumTempus.Application.ProjectTask.Commands.Create;
using ConsiliumTempus.Application.ProjectTask.Commands.Delete;
using ConsiliumTempus.Application.ProjectTask.Commands.Update;
using ConsiliumTempus.Application.ProjectTask.Commands.UpdateOverview;
using ConsiliumTempus.Application.ProjectTask.Queries.Get;
using ConsiliumTempus.Application.ProjectTask.Queries.GetCollection;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.User;
using Mapster;

namespace ConsiliumTempus.Api.Common.Mapping;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class ProjectTaskMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        GetMappings(config);
        GetCollectionMappings(config);
        CreateMappings(config);
        UpdateMappings(config);
        UpdateOverviewMappings(config);
        DeleteMappings(config);
    }

    private static void GetMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetProjectTaskRequest, GetProjectTaskQuery>();

        config.NewConfig<ProjectTaskAggregate, GetProjectTaskResponse>()
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description.Value)
            .Map(dest => dest.IsCompleted, src => src.IsCompleted.Value);
    }

    private static void GetCollectionMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetCollectionProjectTaskRequest, GetCollectionProjectTaskQuery>();

        config.NewConfig<GetCollectionProjectTaskResult, GetCollectionProjectTaskResponse>();
        config.NewConfig<ProjectTaskAggregate, GetCollectionProjectTaskResponse.ProjectTaskResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.IsCompleted, src => src.IsCompleted.Value);
        config.NewConfig<UserAggregate, GetCollectionProjectTaskResponse.UserResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.FirstName.Value + " " + src.LastName.Value)
            .Map(dest => dest.Email, src => src.Credentials.Email);
    }

    private static void CreateMappings(TypeAdapterConfig config)
    {
        config.NewConfig<CreateProjectTaskRequest, CreateProjectTaskCommand>();

        config.NewConfig<CreateProjectTaskResult, CreateProjectTaskResponse>();
    }
    
    private static void UpdateMappings(TypeAdapterConfig config)
    {
        config.NewConfig<UpdateProjectTaskRequest, UpdateProjectTaskCommand>();

        config.NewConfig<UpdateProjectTaskResult, UpdateProjectTaskResponse>();
    }
    
    private static void UpdateOverviewMappings(TypeAdapterConfig config)
    {
        config.NewConfig<UpdateOverviewProjectTaskRequest, UpdateOverviewProjectTaskCommand>();

        config.NewConfig<UpdateOverviewProjectTaskResult, UpdateOverviewProjectTaskResponse>();
    }

    private static void DeleteMappings(TypeAdapterConfig config)
    {
        config.NewConfig<DeleteProjectTaskRequest, DeleteProjectTaskCommand>();

        config.NewConfig<DeleteProjectTaskResult, DeleteProjectTaskResponse>();
    }
}
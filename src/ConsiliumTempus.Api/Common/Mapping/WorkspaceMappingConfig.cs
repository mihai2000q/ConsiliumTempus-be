using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.Contracts.Workspace.Delete;
using ConsiliumTempus.Api.Contracts.Workspace.Get;
using ConsiliumTempus.Api.Contracts.Workspace.Update;
using ConsiliumTempus.Api.Dto;
using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Application.Workspace.Commands.Delete;
using ConsiliumTempus.Application.Workspace.Commands.Update;
using ConsiliumTempus.Application.Workspace.Queries.Get;
using ConsiliumTempus.Domain.Workspace;
using Mapster;

namespace ConsiliumTempus.Api.Common.Mapping;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class WorkspaceMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        GetMappings(config);
        CreateMappings(config);
        PutMappings(config);
        DeleteMappings(config);
    }

    private static void GetMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetWorkspaceRequest, GetWorkspaceQuery>();
        
        config.NewConfig<WorkspaceAggregate, WorkspaceDto>()
            .Map(dest => dest.Id, src => src.Id.Value.ToString())
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description.Value);
    }
    
    private static void CreateMappings(TypeAdapterConfig config)
    {
        config.NewConfig<CreateWorkspaceRequest, CreateWorkspaceCommand>();
        
        config.NewConfig<CreateWorkspaceResult, WorkspaceDto>()
            .Map(dest => dest, src => src.Workspace)
            .Map(dest => dest.Id, src => src.Workspace.Id.Value.ToString())
            .Map(dest => dest.Name, src => src.Workspace.Name.Value)
            .Map(dest => dest.Description, src => src.Workspace.Description.Value);
    }

    private static void PutMappings(TypeAdapterConfig config)
    {
        config.NewConfig<UpdateWorkspaceRequest, UpdateWorkspaceCommand>();
        
        config.NewConfig<UpdateWorkspaceResult, WorkspaceDto>()
            .Map(dest => dest, src => src.Workspace)
            .Map(dest => dest.Id, src => src.Workspace.Id.Value.ToString())
            .Map(dest => dest.Name, src => src.Workspace.Name.Value)
            .Map(dest => dest.Description, src => src.Workspace.Description.Value);
    }

    private static void DeleteMappings(TypeAdapterConfig config)
    {
        config.NewConfig<DeleteWorkspaceResult, DeleteWorkspaceResponse>();
    }
}
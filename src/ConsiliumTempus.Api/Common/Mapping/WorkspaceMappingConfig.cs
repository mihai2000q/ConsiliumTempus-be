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
    public const string Token = "token";
    
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
            .Map(dest => dest.Id, src => src.Id.Value.ToString());
    }
    
    private static void CreateMappings(TypeAdapterConfig config)
    {
        config.NewConfig<CreateWorkspaceRequest, CreateWorkspaceCommand>()
            .Map(dest => dest.Token, 
                _ => MapContext.Current!.Parameters[Token]);
        
        config.NewConfig<CreateWorkspaceResult, WorkspaceDto>()
            .Map(dest => dest, src => src.Workspace)
            .Map(dest => dest.Id, src => src.Workspace.Id.Value.ToString());
    }

    private static void PutMappings(TypeAdapterConfig config)
    {
        config.NewConfig<UpdateWorkspaceRequest, UpdateWorkspaceCommand>();
        
        config.NewConfig<UpdateWorkspaceResult, WorkspaceDto>()
            .Map(dest => dest, src => src.Workspace)
            .Map(dest => dest.Id, src => src.Workspace.Id.Value.ToString());
    }

    private static void DeleteMappings(TypeAdapterConfig config)
    {
        config.NewConfig<DeleteWorkspaceResult, DeleteWorkspaceResponse>();
    }
}
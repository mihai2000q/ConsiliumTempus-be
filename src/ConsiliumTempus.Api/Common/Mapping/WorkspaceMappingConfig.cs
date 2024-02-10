using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.Contracts.Workspace.Get;
using ConsiliumTempus.Api.Dto;
using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Application.Workspace.Queries.Get;
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
    }

    private static void GetMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetWorkspaceRequest, GetWorkspaceQuery>();
        
        config.NewConfig<GetWorkspaceResult, WorkspaceDto>()
            .Map(dest => dest, src => src.Workspace)
            .Map(dest => dest.Id, src => src.Workspace.Id.Value.ToString());
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
}
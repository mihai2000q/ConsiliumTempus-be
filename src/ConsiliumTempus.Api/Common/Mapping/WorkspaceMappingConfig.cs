using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Application.Workspace.Commands.Create;
using Mapster;

namespace ConsiliumTempus.Api.Common.Mapping;

public sealed class WorkspaceMappingConfig : IRegister
{
    public const string Token = "token";
    
    public void Register(TypeAdapterConfig config)
    {
        CreateMappings(config);
    }

    private static void CreateMappings(TypeAdapterConfig config)
    {
        config.NewConfig<CreateWorkspaceRequest, CreateWorkspaceCommand>()
            .Map(dest => dest.Token, 
                _ => MapContext.Current!.Parameters[Token]);

        config.NewConfig<CreateWorkspaceResult, CreateWorkspaceResponse>()
            .Map(dest => dest, src => src.Workspace)
            .Map(dest => dest.Id, src => src.Workspace.Id.Value.ToString());
    }
}
using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Application.Workspace.Command.Create;
using Mapster;

namespace ConsiliumTempus.Api.Common.Mapping;

public sealed class WorkspaceMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        CreateMappings(config);
    }

    private static void CreateMappings(TypeAdapterConfig config)
    {
        config.NewConfig<WorkspaceCreateRequest, WorkspaceCreateCommand>();

        config.NewConfig<WorkspaceCreateResult, WorkspaceCreateResponse>();
    }
}
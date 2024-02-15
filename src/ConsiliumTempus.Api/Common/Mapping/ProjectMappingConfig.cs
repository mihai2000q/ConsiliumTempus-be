using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Application.Project.Commands.Create;
using Mapster;

namespace ConsiliumTempus.Api.Common.Mapping;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class ProjectMappingConfig : IRegister
{
    public const string Token = "token";
    
    public void Register(TypeAdapterConfig config)
    {
        CreateMappings(config);
    }

    private static void CreateMappings(TypeAdapterConfig config)
    {
        config.NewConfig<CreateProjectRequest, CreateProjectCommand>()
            .Map(dest => dest.Token, 
                _ => MapContext.Current!.Parameters[Token]);

        config.NewConfig<CreateProjectResult, CreateProjectResponse>();
    }
}
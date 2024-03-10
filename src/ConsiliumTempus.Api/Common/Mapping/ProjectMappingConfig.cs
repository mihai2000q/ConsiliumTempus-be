using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.Contracts.Project.Delete;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.Project.Commands.Delete;
using Mapster;

namespace ConsiliumTempus.Api.Common.Mapping;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class ProjectMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        CreateMappings(config);
        DeleteMappings(config);
    }

    private static void CreateMappings(TypeAdapterConfig config)
    {
        config.NewConfig<CreateProjectRequest, CreateProjectCommand>();

        config.NewConfig<CreateProjectResult, CreateProjectResponse>();
    }

    private static void DeleteMappings(TypeAdapterConfig config)
    {
        config.NewConfig<DeleteProjectResult, DeleteProjectResponse>();
    }
}
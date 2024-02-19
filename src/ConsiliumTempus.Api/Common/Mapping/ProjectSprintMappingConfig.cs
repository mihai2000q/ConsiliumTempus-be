using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Create;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Delete;
using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Create;
using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Delete;
using Mapster;

namespace ConsiliumTempus.Api.Common.Mapping;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class ProjectSprintMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        CreateMappings(config);
        DeleteMappings(config);
    }

    private static void CreateMappings(TypeAdapterConfig config)
    {
        config.NewConfig<CreateProjectSprintRequest, CreateProjectSprintCommand>();

        config.NewConfig<CreateProjectSprintResult, CreateProjectSprintResponse>();
    }

    private static void DeleteMappings(TypeAdapterConfig config)
    {
        config.NewConfig<DeleteProjectSprintResult, DeleteProjectSprintResponse>();
    }
}
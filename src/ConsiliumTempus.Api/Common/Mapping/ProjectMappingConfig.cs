using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.Contracts.Project.Delete;
using ConsiliumTempus.Api.Contracts.Project.GetCollectionForUser;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Application.Project.Queries.GetCollectionForUser;
using ConsiliumTempus.Domain.Project;
using Mapster;

namespace ConsiliumTempus.Api.Common.Mapping;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class ProjectMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        GetCollectionForUser(config);
        CreateMappings(config);
        DeleteMappings(config);
    }

    private static void GetCollectionForUser(TypeAdapterConfig config)
    {
        config.NewConfig<GetCollectionProjectForUserResult, GetCollectionProjectForUserResponse>();

        config.NewConfig<ProjectAggregate, GetCollectionProjectForUserResponse.ProjectResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value);
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
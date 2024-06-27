using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.Contracts.Workspace.Delete;
using ConsiliumTempus.Api.Contracts.Workspace.Get;
using ConsiliumTempus.Api.Contracts.Workspace.GetCollaborators;
using ConsiliumTempus.Api.Contracts.Workspace.GetCollection;
using ConsiliumTempus.Api.Contracts.Workspace.GetOverview;
using ConsiliumTempus.Api.Contracts.Workspace.Update;
using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Application.Workspace.Commands.Delete;
using ConsiliumTempus.Application.Workspace.Commands.Update;
using ConsiliumTempus.Application.Workspace.Queries.Get;
using ConsiliumTempus.Application.Workspace.Queries.GetCollaborators;
using ConsiliumTempus.Application.Workspace.Queries.GetCollection;
using ConsiliumTempus.Application.Workspace.Queries.GetOverview;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;
using Mapster;

namespace ConsiliumTempus.Api.Common.Mapping;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class WorkspaceMappingConfig : IRegister
{
    public const string CurrentUser = "CurrentUser";

    public void Register(TypeAdapterConfig config)
    {
        GetMappings(config);
        GetOverviewMappings(config);
        GetCollaboratorsMappings(config);
        GetCollectionMappings(config);
        CreateMappings(config);
        UpdateMappings(config);
        DeleteMappings(config);
    }

    private static void GetMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetWorkspaceRequest, GetWorkspaceQuery>();

        config.NewConfig<GetWorkspaceResult, GetWorkspaceResponse>()
            .Map(dest => dest.Name, src => src.Workspace.Name.Value)
            .Map(dest => dest.IsFavorite,
                src => src.Workspace.IsFavorite((UserAggregate)MapContext.Current!.Parameters[CurrentUser]))
            .Map(dest => dest.IsPersonal, src => src.Workspace.IsPersonal.Value);
    }
    
    private static void GetOverviewMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetOverviewWorkspaceRequest, GetOverviewWorkspaceQuery>();

        config.NewConfig<WorkspaceAggregate, GetOverviewWorkspaceResponse>()
            .Map(dest => dest.Description, src => src.Description.Value);
    }

    private static void GetCollaboratorsMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetCollaboratorsFromWorkspaceRequest, GetCollaboratorsFromWorkspaceQuery>();

        config.NewConfig<GetCollaboratorsFromWorkspaceResult, GetCollaboratorsFromWorkspaceResponse>();
        config.NewConfig<UserAggregate, GetCollaboratorsFromWorkspaceResponse.UserResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.FirstName.Value + " " + src.LastName.Value)
            .Map(dest => dest.Email, src => src.Credentials.Email);
    }

    private static void GetCollectionMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetCollectionWorkspaceRequest, GetCollectionWorkspaceQuery>();

        config.NewConfig<GetCollectionWorkspaceResult, GetCollectionWorkspaceResponse>();
        config.NewConfig<WorkspaceAggregate, GetCollectionWorkspaceResponse.WorkspaceResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description.Value)
            .Map(dest => dest.IsFavorite,
                src => src.IsFavorite((UserAggregate)MapContext.Current!.Parameters[CurrentUser]))
            .Map(dest => dest.IsPersonal, src => src.IsPersonal.Value);
        config.NewConfig<UserAggregate, GetCollectionWorkspaceResponse.Owner>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.FirstName.Value + " " + src.LastName.Value);
    }

    private static void CreateMappings(TypeAdapterConfig config)
    {
        config.NewConfig<CreateWorkspaceRequest, CreateWorkspaceCommand>();

        config.NewConfig<CreateWorkspaceResult, CreateWorkspaceResponse>();
    }

    private static void UpdateMappings(TypeAdapterConfig config)
    {
        config.NewConfig<UpdateWorkspaceRequest, UpdateWorkspaceCommand>();

        config.NewConfig<UpdateWorkspaceResult, UpdateWorkspaceResponse>();
    }

    private static void DeleteMappings(TypeAdapterConfig config)
    {
        config.NewConfig<DeleteWorkspaceRequest, DeleteWorkspaceCommand>();

        config.NewConfig<DeleteWorkspaceResult, DeleteWorkspaceResponse>();
    }
}
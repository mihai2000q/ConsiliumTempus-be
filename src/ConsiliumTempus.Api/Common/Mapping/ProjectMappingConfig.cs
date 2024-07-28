using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Api.Contracts.Project.AddStatus;
using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.Contracts.Project.Delete;
using ConsiliumTempus.Api.Contracts.Project.Get;
using ConsiliumTempus.Api.Contracts.Project.GetCollection;
using ConsiliumTempus.Api.Contracts.Project.GetOverview;
using ConsiliumTempus.Api.Contracts.Project.GetStatuses;
using ConsiliumTempus.Api.Contracts.Project.RemoveStatus;
using ConsiliumTempus.Api.Contracts.Project.Update;
using ConsiliumTempus.Api.Contracts.Project.UpdateFavorites;
using ConsiliumTempus.Api.Contracts.Project.UpdateOverview;
using ConsiliumTempus.Api.Contracts.Project.UpdateStatus;
using ConsiliumTempus.Application.Project.Commands.AddStatus;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Application.Project.Commands.RemoveStatus;
using ConsiliumTempus.Application.Project.Commands.Update;
using ConsiliumTempus.Application.Project.Commands.UpdateFavorites;
using ConsiliumTempus.Application.Project.Commands.UpdateOverview;
using ConsiliumTempus.Application.Project.Commands.UpdateStatus;
using ConsiliumTempus.Application.Project.Queries.Get;
using ConsiliumTempus.Application.Project.Queries.GetCollection;
using ConsiliumTempus.Application.Project.Queries.GetOverview;
using ConsiliumTempus.Application.Project.Queries.GetStatuses;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;
using Mapster;

namespace ConsiliumTempus.Api.Common.Mapping;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class ProjectMappingConfig : IRegister
{
    public const string CurrentUser = "CurrentUser";

    public void Register(TypeAdapterConfig config)
    {
        GetMappings(config);
        GetOverviewMappings(config);
        GetCollectionMappings(config);
        GetStatusesMappings(config);
        CreateMappings(config);
        AddStatusMappings(config);
        UpdateMappings(config);
        UpdateFavoritesMappings(config);
        UpdateOverviewMappings(config);
        UpdateStatusMappings(config);
        DeleteMappings(config);
        RemoveStatusMappings(config);
    }

    private static void GetMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetProjectRequest, GetProjectQuery>();

        config.NewConfig<GetProjectResult, GetProjectResponse>()
            .Map(dest => dest.Name, src => src.Project.Name.Value)
            .Map(dest => dest.IsFavorite,
                src => src.Project.IsFavorite((UserAggregate)MapContext.Current!.Parameters[CurrentUser]))
            .Map(dest => dest.Lifecycle, src => src.Project.Lifecycle.ToString())
            .Map(dest => dest.IsPrivate, src => src.Project.IsPrivate.Value)
            .Map(dest => dest.Owner, src => src.Project.Owner)
            .Map(dest => dest.LatestStatus, src => src.Project.LatestStatus)
            .Map(dest => dest.Workspace, src => src.Project.Workspace);
        config.NewConfig<ProjectStatus, GetProjectResponse.ProjectStatusResponse>()
            .IgnoreNullValues(true)
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Title, src => src.Title.Value)
            .Map(dest => dest.Status, src => src.Status.ToString())
            .Map(dest => dest.CreatedBy, src => src.Audit.CreatedBy)
            .Map(dest => dest.CreatedDateTime, src => src.Audit.CreatedDateTime)
            .Map(dest => dest.UpdatedBy, src => src.Audit.UpdatedBy)
            .Map(dest => dest.UpdatedDateTime, src => src.Audit.UpdatedDateTime);
        config.NewConfig<UserAggregate, GetProjectResponse.UserResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Email, src => src.Credentials.Email);
        config.NewConfig<WorkspaceAggregate, GetProjectResponse.WorkspaceResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Id, src => src.Name.Value);
    }

    private static void GetOverviewMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetOverviewProjectRequest, GetOverviewProjectQuery>();

        config.NewConfig<GetOverviewProjectResult, GetOverviewProjectResponse>()
            .Map(dest => dest.Description, src => src.Description.Value);
    }

    private static void GetCollectionMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetCollectionProjectRequest, GetCollectionProjectQuery>();

        config.NewConfig<GetCollectionProjectResult, GetCollectionProjectResponse>();
        config.NewConfig<ProjectAggregate, GetCollectionProjectResponse.ProjectResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description.Value)
            .Map(dest => dest.IsFavorite,
                src => src.IsFavorite((UserAggregate)MapContext.Current!.Parameters[CurrentUser]))
            .Map(dest => dest.Lifecycle, src => src.Lifecycle.ToString())
            .Map(dest => dest.IsPrivate, src => src.IsPrivate.Value);
        config.NewConfig<UserAggregate, GetCollectionProjectResponse.UserResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Email, src => src.Credentials.Email);
        config.NewConfig<ProjectStatus, GetCollectionProjectResponse.ProjectStatusResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.UpdatedDateTime, src => src.Audit.UpdatedDateTime);
    }

    private static void GetStatusesMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetStatusesFromProjectRequest, GetStatusesFromProjectQuery>();

        config.NewConfig<GetStatusesFromProjectResult, GetStatusesFromProjectResponse>();
        config.NewConfig<ProjectStatus, GetStatusesFromProjectResponse.ProjectStatusResponse>()
            .IgnoreNullValues(true)
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Title, src => src.Title.Value)
            .Map(dest => dest.Status, src => src.Status.ToString())
            .Map(dest => dest.Description, src => src.Description.Value)
            .Map(dest => dest.CreatedBy, src => src.Audit.CreatedBy)
            .Map(dest => dest.CreatedDateTime, src => src.Audit.CreatedDateTime)
            .Map(dest => dest.UpdatedBy, src => src.Audit.UpdatedBy)
            .Map(dest => dest.UpdatedDateTime, src => src.Audit.UpdatedDateTime);
        config.NewConfig<UserAggregate, GetStatusesFromProjectResponse.UserResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Email, src => src.Credentials.Email);
    }

    private static void CreateMappings(TypeAdapterConfig config)
    {
        config.NewConfig<CreateProjectRequest, CreateProjectCommand>();

        config.NewConfig<CreateProjectResult, CreateProjectResponse>();
    }

    private static void AddStatusMappings(TypeAdapterConfig config)
    {
        config.NewConfig<AddStatusToProjectRequest, AddStatusToProjectCommand>();

        config.NewConfig<AddStatusToProjectResult, AddStatusToProjectResponse>();
    }

    private static void UpdateMappings(TypeAdapterConfig config)
    {
        config.NewConfig<UpdateProjectRequest, UpdateProjectCommand>();

        config.NewConfig<UpdateProjectResult, UpdateProjectResponse>();
    }

    private static void UpdateFavoritesMappings(TypeAdapterConfig config)
    {
        config.NewConfig<UpdateFavoritesProjectRequest, UpdateFavoritesProjectCommand>();

        config.NewConfig<UpdateFavoritesProjectResult, UpdateFavoritesProjectResponse>();
    }

    private static void UpdateOverviewMappings(TypeAdapterConfig config)
    {
        config.NewConfig<UpdateOverviewProjectRequest, UpdateOverviewProjectCommand>();

        config.NewConfig<UpdateOverviewProjectResult, UpdateOverviewProjectResponse>();
    }

    private static void UpdateStatusMappings(TypeAdapterConfig config)
    {
        config.NewConfig<UpdateStatusFromProjectRequest, UpdateStatusFromProjectCommand>();

        config.NewConfig<UpdateStatusFromProjectResult, UpdateStatusFromProjectResponse>();
    }

    private static void DeleteMappings(TypeAdapterConfig config)
    {
        config.NewConfig<DeleteProjectRequest, DeleteProjectCommand>();

        config.NewConfig<DeleteProjectResult, DeleteProjectResponse>();
    }

    private static void RemoveStatusMappings(TypeAdapterConfig config)
    {
        config.NewConfig<RemoveStatusFromProjectRequest, RemoveStatusFromProjectCommand>();

        config.NewConfig<RemoveStatusFromProjectResult, RemoveStatusFromProjectResponse>();
    }
}
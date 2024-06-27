using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.Contracts.Workspace.Delete;
using ConsiliumTempus.Api.Contracts.Workspace.Get;
using ConsiliumTempus.Api.Contracts.Workspace.GetCollaborators;
using ConsiliumTempus.Api.Contracts.Workspace.GetCollection;
using ConsiliumTempus.Api.Contracts.Workspace.GetOverview;
using ConsiliumTempus.Api.Contracts.Workspace.Update;
using ConsiliumTempus.Common.IntegrationTests.TestConstants;

namespace ConsiliumTempus.Common.IntegrationTests.Workspace;

public static class WorkspaceRequestFactory
{
    public static GetWorkspaceRequest CreateGetWorkspaceRequest(Guid? id = null)
    {
        return new GetWorkspaceRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }
    
    public static GetOverviewWorkspaceRequest CreateGetOverviewWorkspaceRequest(Guid? id = null)
    {
        return new GetOverviewWorkspaceRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }


    public static GetCollaboratorsFromWorkspaceRequest CreateGetCollaboratorsFromWorkspaceRequest(
        Guid? id = null,
        string searchValue = "")
    {
        return new GetCollaboratorsFromWorkspaceRequest
        {
            Id = id ?? Guid.NewGuid(),
            SearchValue = searchValue
        };
    }

    public static GetCollectionWorkspaceRequest CreateGetCollectionWorkspaceRequest(
        bool isPersonalWorkspaceFirst = false,
        int? pageSize = null,
        int? currentPage = null,
        string[]? orderBy = null,
        string[]? search = null)
    {
        return new GetCollectionWorkspaceRequest
        {
            IsPersonalWorkspaceFirst = isPersonalWorkspaceFirst,
            PageSize = pageSize,
            CurrentPage = currentPage,
            OrderBy = orderBy,
            Search = search
        };
    }

    public static CreateWorkspaceRequest CreateCreateWorkspaceRequest(
        string name = Constants.Workspace.Name)
    {
        return new CreateWorkspaceRequest(
            name);
    }

    public static UpdateWorkspaceRequest CreateUpdateWorkspaceRequest(
        Guid? id = null,
        string name = Constants.Workspace.Name,
        bool isFavorite = false)
    {
        return new UpdateWorkspaceRequest(
            id ?? Guid.NewGuid(),
            name,
            isFavorite);
    }

    public static DeleteWorkspaceRequest CreateDeleteWorkspaceRequest(Guid? id = null)
    {
        return new DeleteWorkspaceRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }
}
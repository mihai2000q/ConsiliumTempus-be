using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.Contracts.Project.Delete;
using ConsiliumTempus.Api.Contracts.Project.Get;
using ConsiliumTempus.Api.Contracts.Project.GetCollection;
using ConsiliumTempus.Api.Contracts.Project.GetOverview;
using ConsiliumTempus.Api.Contracts.Project.Update;
using ConsiliumTempus.Common.IntegrationTests.TestConstants;

namespace ConsiliumTempus.Common.IntegrationTests.Project;

public static class ProjectRequestFactory
{
    public static GetProjectRequest CreateGetProjectRequest(Guid? id = null)
    {
        return new GetProjectRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }
    
    public static GetOverviewProjectRequest CreateGetOverviewProjectRequest(Guid? id = null)
    {
        return new GetOverviewProjectRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }

    public static GetCollectionProjectRequest CreateGetCollectionProjectRequest(
        int? pageSize = null,
        int? currentPage = null,
        string? order = null,
        Guid? workspaceId = null,
        string? name = null,
        bool? isFavorite = null,
        bool? isPrivate = null)
    {
        return new GetCollectionProjectRequest
        {
            PageSize = pageSize,
            CurrentPage = currentPage,
            Order = order,
            WorkspaceId = workspaceId,
            Name = name,
            IsFavorite = isFavorite,
            IsPrivate = isPrivate
        };
    }

    public static CreateProjectRequest CreateCreateProjectRequest(
        Guid? workspaceId = null,
        string name = Constants.Project.Name,
        bool isPrivate = false)
    {
        return new CreateProjectRequest(
            workspaceId ?? Guid.NewGuid(),
            name,
            isPrivate);
    }
    
    public static UpdateProjectRequest CreateUpdateProjectRequest(
        Guid? id = null,
        string name = Constants.Project.Name,
        bool isFavorite = false)
    {
        return new UpdateProjectRequest(
            id ?? Guid.NewGuid(),
            name,
            isFavorite);
    }
    
    public static DeleteProjectRequest CreateDeleteProjectRequest(Guid? id = null)
    {
        return new DeleteProjectRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }
}
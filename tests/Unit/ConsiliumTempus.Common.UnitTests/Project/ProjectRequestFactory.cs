using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.Contracts.Project.Get;
using ConsiliumTempus.Api.Contracts.Project.GetCollection;
using ConsiliumTempus.Common.UnitTests.TestConstants;

namespace ConsiliumTempus.Common.UnitTests.Project;

public static class ProjectRequestFactory
{
    public static GetProjectRequest CreateGetProjectRequest(
        Guid? id = null)
    {
        return new GetProjectRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }

    public static GetCollectionProjectRequest CreateGetCollectionProjectRequest(
        string? order = null,
        Guid? workspaceId = null,
        string? name = null,
        bool? isFavorite = null,
        bool? isPrivate = null)
    {
        return new GetCollectionProjectRequest
        {
            Order = order,
            WorkspaceId = workspaceId,
            Name = name,
            IsFavorite = isFavorite,
            IsPrivate = isPrivate,
        };
    }

    public static CreateProjectRequest CreateCreateProjectRequest(
        Guid? workspaceId = null,
        string name = Constants.Project.Name,
        string description = Constants.Project.Description,
        bool isPrivate = false)
    {
        return new CreateProjectRequest(
            workspaceId ?? Guid.NewGuid(),
            name,
            description,
            isPrivate);
    }
}
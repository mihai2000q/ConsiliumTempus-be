namespace ConsiliumTempus.Api.Contracts.Project.GetCollectionForUser;

public sealed record GetCollectionProjectForUserResponse(
    List<GetCollectionProjectForUserResponse.ProjectResponse> Projects)
{
    public sealed record ProjectResponse(
        Guid Id,
        string Name);
}
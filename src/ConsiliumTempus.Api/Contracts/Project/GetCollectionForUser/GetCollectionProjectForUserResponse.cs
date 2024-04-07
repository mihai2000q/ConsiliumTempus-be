using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Project.GetCollectionForUser;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetCollectionProjectForUserResponse(
    List<GetCollectionProjectForUserResponse.ProjectResponse> Projects)
{
    public sealed record ProjectResponse(
        Guid Id,
        string Name);
}
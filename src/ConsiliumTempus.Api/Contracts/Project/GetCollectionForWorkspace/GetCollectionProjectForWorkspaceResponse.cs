using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Project.GetCollectionForWorkspace;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetCollectionProjectForWorkspaceResponse(
    List<GetCollectionProjectForWorkspaceResponse.ProjectResponse> Projects)
{
    public sealed record ProjectResponse(
        Guid Id,
        string Name,
        string Description);
}
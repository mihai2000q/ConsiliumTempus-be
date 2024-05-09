using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Application.Workspace.Queries.GetCollection;

public sealed record GetCollectionWorkspaceResult(
    List<WorkspaceAggregate> Workspaces,
    int TotalCount,
    int? TotalPages);
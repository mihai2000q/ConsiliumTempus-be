using ConsiliumTempus.Domain.Project;

namespace ConsiliumTempus.Application.Project.Queries.GetCollectionForWorkspace;

public sealed record GetCollectionProjectForWorkspaceResult(List<ProjectAggregate> Projects);
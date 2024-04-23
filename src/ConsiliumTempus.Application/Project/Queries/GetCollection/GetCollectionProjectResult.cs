using ConsiliumTempus.Domain.Project;

namespace ConsiliumTempus.Application.Project.Queries.GetCollection;

public sealed record GetCollectionProjectResult(List<ProjectAggregate> Projects);
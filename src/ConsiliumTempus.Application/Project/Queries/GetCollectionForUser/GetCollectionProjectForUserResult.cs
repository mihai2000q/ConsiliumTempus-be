using ConsiliumTempus.Domain.Project;

namespace ConsiliumTempus.Application.Project.Queries.GetCollectionForUser;

public sealed record GetCollectionProjectForUserResult(List<ProjectAggregate> Projects);
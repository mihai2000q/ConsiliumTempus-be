using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Application.Project.Queries.GetCollection;

public sealed record GetCollectionProjectResult(
    List<ProjectAggregate> Projects,
    int TotalCount,
    UserAggregate CurrentUser);
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Application.Project.Queries.Get;

public sealed record GetProjectResult(
    ProjectAggregate Project,
    UserAggregate CurrentUser);
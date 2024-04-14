using ConsiliumTempus.Domain.Project;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Queries.Get;

public sealed record GetProjectQuery(Guid Id) : IRequest<ErrorOr<ProjectAggregate>>;
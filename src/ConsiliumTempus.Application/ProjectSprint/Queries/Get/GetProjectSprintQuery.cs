using ConsiliumTempus.Domain.ProjectSprint;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectSprint.Queries.Get;

public sealed record GetProjectSprintQuery(Guid Id) : IRequest<ErrorOr<ProjectSprintAggregate>>;
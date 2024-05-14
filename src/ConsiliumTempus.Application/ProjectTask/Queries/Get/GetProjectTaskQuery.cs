using ConsiliumTempus.Domain.ProjectTask;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectTask.Queries.Get;

public sealed record GetProjectTaskQuery(
    Guid Id)
    : IRequest<ErrorOr<ProjectTaskAggregate>>;
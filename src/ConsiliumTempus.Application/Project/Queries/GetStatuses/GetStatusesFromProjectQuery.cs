using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Queries.GetStatuses;

public sealed record GetStatusesFromProjectQuery(
    Guid Id)
    : IRequest<ErrorOr<GetStatusesFromProjectResult>>;
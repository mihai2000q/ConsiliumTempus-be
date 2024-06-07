using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Queries.GetStatuses;

public record GetStatusesFromProjectQuery(
    Guid Id)
    : IRequest<ErrorOr<GetStatusesFromProjectResult>>;
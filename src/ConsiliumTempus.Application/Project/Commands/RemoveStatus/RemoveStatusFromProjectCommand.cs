using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.RemoveStatus;

public sealed record RemoveStatusFromProjectCommand(
    Guid Id,
    Guid StatusId)
    : IRequest<ErrorOr<RemoveStatusFromProjectResult>>;
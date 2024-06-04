using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.UpdateStatus;

public sealed record UpdateStatusFromProjectCommand(
    Guid Id,
    Guid StatusId,
    string Title,
    string Status,
    string Description)
    : IRequest<ErrorOr<UpdateStatusFromProjectResult>>;
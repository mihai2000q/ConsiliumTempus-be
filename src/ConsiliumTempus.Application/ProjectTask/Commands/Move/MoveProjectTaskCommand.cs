using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectTask.Commands.Move;

public sealed record MoveProjectTaskCommand(
    Guid SprintId,
    Guid Id,
    Guid OverId)
    : IRequest<ErrorOr<MoveProjectTaskResult>>;
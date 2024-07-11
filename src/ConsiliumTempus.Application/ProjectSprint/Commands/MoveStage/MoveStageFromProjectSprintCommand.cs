using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectSprint.Commands.MoveStage;

public sealed record MoveStageFromProjectSprintCommand(
    Guid Id,
    Guid StageId,
    Guid OverStageId)
    : IRequest<ErrorOr<MoveStageFromProjectSprintResult>>;
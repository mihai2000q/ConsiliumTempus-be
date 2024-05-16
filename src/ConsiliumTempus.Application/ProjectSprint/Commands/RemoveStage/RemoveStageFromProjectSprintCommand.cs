using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectSprint.Commands.RemoveStage;

public sealed record RemoveStageFromProjectSprintCommand(
    Guid ProjectSprintId,
    Guid StageId)
    : IRequest<ErrorOr<RemoveStageFromProjectSprintResult>>;

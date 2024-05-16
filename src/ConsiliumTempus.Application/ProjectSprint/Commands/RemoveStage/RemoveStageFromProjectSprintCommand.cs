using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectSprint.Commands.RemoveStage;

public sealed record RemoveStageFromProjectSprintCommand(
    Guid Id,
    Guid StageId)
    : IRequest<ErrorOr<RemoveStageFromProjectSprintResult>>;

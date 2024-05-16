using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectSprint.Commands.UpdateStage;

public sealed record UpdateStageFromProjectSprintCommand(
    Guid ProjectSprintId,
    Guid StageId,
    string Name)
    : IRequest<ErrorOr<UpdateStageFromProjectSprintResult>>;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Entities.Stage.Commands.Create;

public sealed record CreateProjectStageCommand(
    Guid ProjectSprintId,
    string Name)
    : IRequest<ErrorOr<CreateProjectStageResult>>;
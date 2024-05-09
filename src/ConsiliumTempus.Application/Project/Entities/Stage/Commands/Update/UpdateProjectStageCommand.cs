using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Entities.Stage.Commands.Update;

public sealed record UpdateProjectStageCommand(
    Guid Id,
    string Name) 
    : IRequest<ErrorOr<UpdateProjectStageResult>>;
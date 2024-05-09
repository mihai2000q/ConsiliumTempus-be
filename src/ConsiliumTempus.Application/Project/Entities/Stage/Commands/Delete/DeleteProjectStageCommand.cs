using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Entities.Stage.Commands.Delete;

public sealed record DeleteProjectStageCommand(Guid Id) : IRequest<ErrorOr<DeleteProjectStageResult>>;
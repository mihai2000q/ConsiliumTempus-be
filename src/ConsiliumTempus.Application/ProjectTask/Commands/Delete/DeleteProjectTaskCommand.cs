using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectTask.Commands.Delete;

public sealed record DeleteProjectTaskCommand(
    Guid Id,
    Guid StageId)
    : IRequest<ErrorOr<DeleteProjectTaskResult>>;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectTask.Commands.Delete;

public sealed record DeleteProjectTaskCommand(Guid Id)
    : IRequest<ErrorOr<DeleteProjectTaskResult>>;
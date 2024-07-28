using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectTask.Commands.UpdateIsCompleted;

public sealed record UpdateIsCompletedProjectTaskCommand(
    Guid Id,
    bool IsCompleted)
    : IRequest<ErrorOr<UpdateIsCompletedProjectTaskResult>>;
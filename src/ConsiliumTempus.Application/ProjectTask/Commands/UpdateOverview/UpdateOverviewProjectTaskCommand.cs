using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectTask.Commands.UpdateOverview;

public sealed record UpdateOverviewProjectTaskCommand(
    Guid Id,
    string Name,
    string Description,
    bool IsCompleted,
    Guid? AssigneeId)
    : IRequest<ErrorOr<UpdateOverviewProjectTaskResult>>;
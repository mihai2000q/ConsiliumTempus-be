using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectTask.Commands.Update;

public sealed record UpdateProjectTaskCommand(
    Guid Id,
    string Name,
    Guid? AssigneeId)
    : IRequest<ErrorOr<UpdateProjectTaskResult>>;
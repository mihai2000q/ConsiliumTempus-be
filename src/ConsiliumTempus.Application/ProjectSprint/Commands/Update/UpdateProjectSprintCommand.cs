using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectSprint.Commands.Update;

public sealed record UpdateProjectSprintCommand(
    Guid Id,
    string Name,
    DateOnly? StartDate,
    DateOnly? EndDate)
    : IRequest<ErrorOr<UpdateProjectSprintResult>>;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Update;

public sealed record UpdateProjectSprintCommand(
    Guid Id,
    string Name,
    DateOnly? StartDate,
    DateOnly? EndDate)
    : IRequest<ErrorOr<UpdateProjectSprintResult>>;
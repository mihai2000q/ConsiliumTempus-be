using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectSprint.Commands.Create;

public sealed record CreateProjectSprintCommand(
    Guid ProjectId,
    string Name,
    DateOnly? StartDate,
    DateOnly? EndDate)
    : IRequest<ErrorOr<CreateProjectSprintResult>>;
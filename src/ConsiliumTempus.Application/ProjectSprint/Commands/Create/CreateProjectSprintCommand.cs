using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectSprint.Commands.Create;

public sealed record CreateProjectSprintCommand(
    Guid ProjectId,
    string Name,
    DateOnly? StartDate,
    DateOnly? EndDate,
    bool KeepPreviousStages, 
    CreateProjectSprintCommand.CreateProjectStatus? ProjectStatus)
    : IRequest<ErrorOr<CreateProjectSprintResult>>
{
    public sealed record CreateProjectStatus(
        string Title,
        string Status,
        string Description);
}
using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Create;

public sealed class CreateProjectSprintCommandHandler(
    IProjectRepository projectRepository,
    IProjectSprintRepository projectSprintRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateProjectSprintCommand, ErrorOr<CreateProjectSprintResult>>
{
    public async Task<ErrorOr<CreateProjectSprintResult>> Handle(CreateProjectSprintCommand command,
        CancellationToken cancellationToken)
    {
        var projectId = ProjectId.Create(command.ProjectId);
        var project = await projectRepository.GetWithWorkspace(projectId, cancellationToken);

        if (project is null) return Errors.Project.NotFound;

        var projectSprint = Domain.Project.Entities.ProjectSprint.Create(
            Name.Create(command.Name),
            project,
            command.StartDate,
            command.EndDate);
        await projectSprintRepository.Add(projectSprint, cancellationToken);

        project.RefreshUpdatedDateTime();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateProjectSprintResult();
    }
}
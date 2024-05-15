using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.Delete;

public sealed class DeleteProjectCommandHandler(IProjectRepository projectRepository)
    : IRequestHandler<DeleteProjectCommand, ErrorOr<DeleteProjectResult>>
{
    public async Task<ErrorOr<DeleteProjectResult>> Handle(DeleteProjectCommand command,
        CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetWithWorkspace(ProjectId.Create(command.Id), cancellationToken);
        if (project is null) return Errors.Project.NotFound;

        projectRepository.Remove(project);

        project.Workspace.RefreshActivity();

        return new DeleteProjectResult();
    }
}
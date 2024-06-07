using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.RemoveStatus;

public sealed class RemoveStatusFromProjectCommandHandler(IProjectRepository projectRepository)
    : IRequestHandler<RemoveStatusFromProjectCommand, ErrorOr<RemoveStatusFromProjectResult>>
{
    public async Task<ErrorOr<RemoveStatusFromProjectResult>> Handle(RemoveStatusFromProjectCommand command,
        CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetWithWorkspace(
            ProjectId.Create(command.Id),
            cancellationToken);
        if (project is null) return Errors.Project.NotFound;

        var status = project.Statuses.SingleOrDefault(s => s.Id.Value == command.StatusId);
        if (status is null) return Errors.ProjectStatus.NotFound;

        project.RemoveStatus(status);
        project.RefreshActivity();

        return new RemoveStatusFromProjectResult();
    }
}
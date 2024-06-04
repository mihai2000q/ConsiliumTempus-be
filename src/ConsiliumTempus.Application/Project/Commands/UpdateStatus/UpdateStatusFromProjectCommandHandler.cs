using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.Enums;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.UpdateStatus;

public sealed class UpdateStatusFromProjectCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IProjectRepository projectRepository)
    : IRequestHandler<UpdateStatusFromProjectCommand, ErrorOr<UpdateStatusFromProjectResult>>
{
    public async Task<ErrorOr<UpdateStatusFromProjectResult>> Handle(UpdateStatusFromProjectCommand command,
        CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetWithWorkspace(
            ProjectId.Create(command.Id),
            cancellationToken);
        if (project is null) return Errors.Project.NotFound;

        var status = project.Statuses.SingleOrDefault(s => s.Id.Value == command.StatusId);
        if (status is null) return Errors.ProjectStatus.NotFound;

        var user = await currentUserProvider.GetCurrentUserAfterPermissionCheck(cancellationToken);

        status.Update(
            Title.Create(command.Title),
            Enum.Parse<ProjectStatusType>(command.Status),
            Description.Create(command.Description),
            user);

        return new UpdateStatusFromProjectResult();
    }
}
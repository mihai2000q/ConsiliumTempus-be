using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.Enums;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.AddStatus;

public sealed class AddStatusToProjectCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IProjectRepository projectRepository)
    : IRequestHandler<AddStatusToProjectCommand, ErrorOr<AddStatusToProjectResult>>
{
    public async Task<ErrorOr<AddStatusToProjectResult>> Handle(AddStatusToProjectCommand command,
        CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetWithWorkspace(
            ProjectId.Create(command.Id),
            cancellationToken);
        if (project is null) return Errors.Project.NotFound;

        var user = await currentUserProvider.GetCurrentUserAfterPermissionCheck(cancellationToken);

        project.AddStatus(ProjectStatus.Create(
            Title.Create(command.Title),
            Enum.Parse<ProjectStatusType>(command.Status),
            Description.Create(command.Description),
            project,
            user));

        project.RefreshActivity();

        return new AddStatusToProjectResult();
    }
}
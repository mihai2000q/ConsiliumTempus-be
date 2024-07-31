using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.LeavePrivate;

public sealed class LeavePrivateProjectCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IProjectRepository projectRepository)
    : IRequestHandler<LeavePrivateProjectCommand, ErrorOr<LeavePrivateProjectResult>>
{
    public async Task<ErrorOr<LeavePrivateProjectResult>> Handle(LeavePrivateProjectCommand command,
        CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetWithAllowedMembers(ProjectId.Create(command.Id), cancellationToken);
        if (project is null) return Errors.Project.NotFound;
        if (!project.IsPrivate.Value) return Errors.Project.NotPrivate;

        var user = await currentUserProvider.GetCurrentUserAfterPermissionCheck(cancellationToken);
        if (project.Owner == user) return Errors.Project.LeaveOwned;

        project.RemoveAllowedMember(user);
        project.RefreshActivity();

        return new LeavePrivateProjectResult();
    }
}
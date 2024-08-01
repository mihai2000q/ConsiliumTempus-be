using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.RemoveAllowedMember;

public sealed class RemoveAllowedMemberFromProjectCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IProjectRepository projectRepository)
    : IRequestHandler<RemoveAllowedMemberFromProjectCommand, ErrorOr<RemoveAllowedMemberFromProjectResult>>
{
    public async Task<ErrorOr<RemoveAllowedMemberFromProjectResult>> Handle(
        RemoveAllowedMemberFromProjectCommand command,
        CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetWithAllowedMembers(ProjectId.Create(command.Id), cancellationToken);
        if (project is null) return Errors.Project.NotFound;
        if (!project.IsPrivate.Value) return Errors.Project.NotPrivate;
        
        var allowedMember = project.AllowedMembers
            .SingleOrDefault(u => u.Id.Value == command.AllowedMemberId);
        if (allowedMember is null) return Errors.Project.AllowedMemberNotFound;
        if (allowedMember == project.Owner) return Errors.Project.RemoveOwner;

        var user = await currentUserProvider.GetCurrentUserAfterPermissionCheck(cancellationToken);
        if (allowedMember == user) return Errors.Project.RemoveYourself;

        project.RemoveAllowedMember(allowedMember);
        project.RefreshActivity();
        return new RemoveAllowedMemberFromProjectResult();
    }
}
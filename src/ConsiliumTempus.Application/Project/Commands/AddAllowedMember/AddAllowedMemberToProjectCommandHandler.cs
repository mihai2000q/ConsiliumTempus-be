using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.AddAllowedMember;

public sealed class AddAllowedMemberToProjectCommandHandler(IProjectRepository projectRepository)
    : IRequestHandler<AddAllowedMemberToProjectCommand, ErrorOr<AddAllowedMemberToProjectResult>>
{
    public async Task<ErrorOr<AddAllowedMemberToProjectResult>> Handle(AddAllowedMemberToProjectCommand command,
        CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetWithCollaboratorsAndAllowedMembers(
            ProjectId.Create(command.Id),
            cancellationToken);
        if (project is null) return Errors.Project.NotFound;
        if (!project.IsPrivate.Value) return Errors.Project.NotPrivate;

        var collaborator = project.Workspace.Memberships
            .SingleOrDefault(m => m.User.Id.Value == command.CollaboratorId)
            ?.User;
        if (collaborator is null) return Errors.Workspace.CollaboratorNotFound;
        if (project.AllowedMembers.Contains(collaborator)) return Errors.Project.AlreadyAllowedMember;

        project.AddAllowedMember(collaborator);
        project.RefreshActivity();

        return new AddAllowedMemberToProjectResult();
    }
}
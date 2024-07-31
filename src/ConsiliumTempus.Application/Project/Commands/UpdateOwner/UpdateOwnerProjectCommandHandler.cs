using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.UpdateOwner;

public sealed class UpdateOwnerProjectCommandHandler(IProjectRepository projectRepository)
    : IRequestHandler<UpdateOwnerProjectCommand, ErrorOr<UpdateOwnerProjectResult>>
{
    public async Task<ErrorOr<UpdateOwnerProjectResult>> Handle(UpdateOwnerProjectCommand command,
        CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetWithCollaborators(ProjectId.Create(command.Id), cancellationToken);
        if (project is null) return Errors.Project.NotFound;

        var collaborator = project.Workspace.Memberships
            .SingleOrDefault(m => m.User.Id.Value == command.OwnerId)
            ?.User;
        if (collaborator is null) return Errors.Workspace.CollaboratorNotFound;

        project.UpdateOwner(collaborator);

        return new UpdateOwnerProjectResult();
    }
}
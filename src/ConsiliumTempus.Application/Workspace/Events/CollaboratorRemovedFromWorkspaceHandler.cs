using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Project.Events;
using ConsiliumTempus.Domain.Workspace.Events;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Events;

internal sealed class CollaboratorRemovedFromWorkspaceHandler(IProjectRepository projectRepository)
    : INotificationHandler<CollaboratorRemovedFromWorkspace>
{
    public async Task Handle(CollaboratorRemovedFromWorkspace notification, CancellationToken cancellationToken)
    {
        var (workspace, collaborator) = notification;
        if (workspace.IsFavorite(collaborator)) workspace.UpdateFavorites(false, collaborator);

        var projects = await projectRepository.GetRelatedListByUserAndWorkspace(
            collaborator.Id,
            workspace.Id,
            cancellationToken);

        foreach (var project in projects)
        {
            var isLastAllowedMember = project.AllowedMembers.Count == 1 && project.AllowedMembers.Contains(collaborator);
            if (project.IsPrivate.Value && isLastAllowedMember)
            {
                projectRepository.Remove(project);
                project.AddDomainEvent(new ProjectDeleted(project));
                continue;
            }

            if (project.AllowedMembers.Contains(collaborator))
                project.RemoveAllowedMember(collaborator);
            else if (project.IsFavorite(collaborator))
                project.UpdateFavorites(false, collaborator);

            if (project.Owner != collaborator) continue;
            var newOwner = workspace.NextProjectOwner();
            project.UpdateOwner(newOwner);
            if (!project.AllowedMembers.Contains(newOwner)) project.AddAllowedMember(newOwner);
        }
    }
}
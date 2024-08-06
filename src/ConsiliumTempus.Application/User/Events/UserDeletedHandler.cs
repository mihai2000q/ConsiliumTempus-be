using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.Events;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using MediatR;

namespace ConsiliumTempus.Application.User.Events;

public sealed class UserDeletedHandler(
    IUserRepository userRepository,
    IWorkspaceRepository workspaceRepository,
    IProjectRepository projectRepository)
    : INotificationHandler<UserDeleted>
{
    public async Task Handle(UserDeleted notification, CancellationToken cancellationToken)
    {
        await UpdateOwnedWorkspaces(notification.User, cancellationToken);
        await UpdateOwnedProjects(notification.User, cancellationToken);
        await userRepository.NullifyAuditsByUser(notification.User, cancellationToken);
        await userRepository.RemoveWorkspaceInvitationsByUser(notification.User, cancellationToken);
    }

    private async Task UpdateOwnedWorkspaces(UserAggregate user, CancellationToken cancellationToken)
    {
        var workspaces = await workspaceRepository.GetListByOwner(user.Id, cancellationToken);
        foreach (var workspace in workspaces)
        {
            if (workspace.Memberships.Count == 1)
            {
                workspaceRepository.Remove(workspace);
                continue;
            }

            var newOwner = workspace.Memberships
                .OrderByDescending(m => m.WorkspaceRole.Id)
                .First(m => m.User != user);
            if (newOwner.WorkspaceRole != WorkspaceRole.Admin) newOwner.Update(WorkspaceRole.Admin);
            workspace.UpdateOwner(newOwner.User);
            workspace.UpdateIsPersonal(IsPersonal.Create(false));
        }
    }

    private async Task UpdateOwnedProjects(UserAggregate user, CancellationToken cancellationToken)
    {
        var projects = await projectRepository.GetListByOwner(user.Id, cancellationToken);
        foreach (var project in projects.Where(project => project.Workspace.Memberships.Count > 1))
        {
            if (project.IsPrivate.Value && project.AllowedMembers.Count == 1)
            {
                projectRepository.Remove(project);
                continue;
            }

            var newOwner = project.IsPrivate.Value
                ? project.AllowedMembers.First(u => u != user)
                : project.Workspace.Memberships
                    .OrderByDescending(m => m.WorkspaceRole.Id)
                    .First(m => m.User != user)
                    .User;
            project.UpdateOwner(newOwner);
            if (!project.AllowedMembers.Contains(newOwner)) project.AddAllowedMember(newOwner);
        }
    }
}
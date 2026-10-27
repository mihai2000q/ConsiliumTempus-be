using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Workspace.Events;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Events;

internal sealed class WorkspaceDeletedHandler(
    IProjectRepository projectRepository,
    ICustomFieldSetupRepository customFieldSetupRepository)
    : INotificationHandler<WorkspaceDeleted>
{
    public async Task Handle(WorkspaceDeleted notification, CancellationToken cancellationToken)
    {
        var workspace = notification.Workspace;

        // Get projects, because custom field setups might not be global, but local
        var projects = await projectRepository.GetListByWorkspace(workspace.Id, cancellationToken);
        await customFieldSetupRepository.DeleteByWorkspaceOrProjects(workspace, projects, cancellationToken);
    }
}
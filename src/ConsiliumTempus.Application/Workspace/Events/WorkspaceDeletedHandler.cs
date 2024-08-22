using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Workspace.Events;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Events;

public sealed class WorkspaceDeletedHandler(
    IProjectRepository projectRepository,
    IProjectTaskRepository projectTaskRepository,
    ICustomFieldSetupRepository customFieldSetupRepository)
    : INotificationHandler<WorkspaceDeleted>
{
    public async Task Handle(WorkspaceDeleted notification, CancellationToken cancellationToken)
    {
        var workspace = notification.Workspace;

        await projectTaskRepository.DeleteCustomFieldsByWorkspace(workspace, cancellationToken);
        // Get projects, because custom field setups might not be global, but local
        var projects = await projectRepository.GetListByWorkspace(workspace.Id, cancellationToken);
        await customFieldSetupRepository.DeleteByWorkspaceOrProjects(workspace, projects, cancellationToken);
    }
}
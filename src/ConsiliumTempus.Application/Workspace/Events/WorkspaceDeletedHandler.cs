using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Workspace.Events;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Events;

public sealed class WorkspaceDeletedHandler(
    IProjectRepository projectRepository,
    ICustomFieldSetupRepository customFieldSetupRepository)
    : INotificationHandler<WorkspaceDeleted>
{
    public async Task Handle(WorkspaceDeleted notification, CancellationToken cancellationToken)
    {
        var workspace = notification.Workspace;

        var projects = await projectRepository.GetListByWorkspace(workspace.Id, cancellationToken);
        var customFieldSetups = await customFieldSetupRepository.GetListByWorkspaceOrProjects(
            workspace.Id,
            projects,
            cancellationToken);
        customFieldSetupRepository.RemoveRange(customFieldSetups);
    }
}
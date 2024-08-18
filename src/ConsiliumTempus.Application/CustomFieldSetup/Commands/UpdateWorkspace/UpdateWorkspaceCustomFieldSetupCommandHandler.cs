using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.CustomFieldSetup.Commands.UpdateWorkspace;

public sealed class UpdateWorkspaceCustomFieldSetupCommandHandler(
    ICustomFieldSetupRepository customFieldSetupRepository,
    IWorkspaceRepository workspaceRepository,
    ICurrentUserProvider currentUserProvider)
    : IRequestHandler<UpdateWorkspaceCustomFieldSetupCommand, ErrorOr<UpdateWorkspaceCustomFieldSetupResult>>
{
    public async Task<ErrorOr<UpdateWorkspaceCustomFieldSetupResult>> Handle(
        UpdateWorkspaceCustomFieldSetupCommand command,
        CancellationToken cancellationToken)
    {
        var customFieldSetup = await customFieldSetupRepository.GetWithWorkspace(
            CustomFieldSetupId.Create(command.Id),
            cancellationToken);
        if (customFieldSetup is null) return Errors.CustomFieldSetup.NotFound;

        var workspace = await workspaceRepository.Get(WorkspaceId.Create(command.WorkspaceId), cancellationToken);
        if (workspace is null) return Errors.Workspace.NotFound;

        var user = await currentUserProvider.GetCurrentUserAfterPermissionCheck(cancellationToken);

        customFieldSetup.UpdateWorkspace(workspace, user);
        workspace.RefreshActivity();

        return new UpdateWorkspaceCustomFieldSetupResult();
    }
}
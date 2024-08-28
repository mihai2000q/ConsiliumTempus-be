using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.CustomFieldSetup.Commands.MakeGlobal;

public sealed class MakeCustomFieldSetupGlobalCommandHandler(
    ICustomFieldSetupRepository customFieldSetupRepository,
    ICurrentUserProvider currentUserProvider)
    : IRequestHandler<MakeCustomFieldSetupGlobalCommand, ErrorOr<MakeCustomFieldSetupGlobalResult>>
{
    public async Task<ErrorOr<MakeCustomFieldSetupGlobalResult>> Handle(
        MakeCustomFieldSetupGlobalCommand command,
        CancellationToken cancellationToken)
    {
        var customFieldSetup = await customFieldSetupRepository.GetWithWorkspaceAndProjects(
            CustomFieldSetupId.Create(command.Id),
            cancellationToken);
        if (customFieldSetup is null) return Errors.CustomFieldSetup.NotFound;
        if (customFieldSetup.Workspace is not null) return Errors.CustomFieldSetup.AlreadyGlobal;

        var user = await currentUserProvider.GetCurrentUserAfterPermissionCheck(cancellationToken);
        var workspace = customFieldSetup.Projects.Single().Workspace;

        customFieldSetup.UpdateWorkspace(workspace, user);
        customFieldSetup.Workspace?.RefreshActivity();

        return new MakeCustomFieldSetupGlobalResult();
    }
}
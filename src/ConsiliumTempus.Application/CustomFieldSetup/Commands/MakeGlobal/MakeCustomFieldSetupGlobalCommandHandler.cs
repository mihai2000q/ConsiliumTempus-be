using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.CustomFieldSetup.Commands.MakeGlobal;

internal sealed class MakeCustomFieldSetupGlobalCommandHandler(
    ICustomFieldSetupRepository customFieldSetupRepository)
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
        if (customFieldSetup.IsGlobal) return Errors.CustomFieldSetup.AlreadyGlobal;

        var workspace = customFieldSetup.Projects.Single().Workspace;
        customFieldSetup.UpdateWorkspace(workspace);
        workspace.RefreshActivity();

        return new MakeCustomFieldSetupGlobalResult();
    }
}
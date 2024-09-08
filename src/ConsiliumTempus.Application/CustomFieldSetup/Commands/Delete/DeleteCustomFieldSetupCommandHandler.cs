using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.CustomFieldSetup.Events;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.CustomFieldSetup.Commands.Delete;

public sealed class DeleteCustomFieldSetupCommandHandler(ICustomFieldSetupRepository customFieldSetupRepository)
    : IRequestHandler<DeleteCustomFieldSetupCommand, ErrorOr<DeleteCustomFieldSetupResult>>
{
    public async Task<ErrorOr<DeleteCustomFieldSetupResult>> Handle(DeleteCustomFieldSetupCommand command,
        CancellationToken cancellationToken)
    {
        var customFieldSetup = await customFieldSetupRepository.GetWithWorkspaceAndProjects(
            CustomFieldSetupId.Create(command.Id),
            cancellationToken);
        if (customFieldSetup is null) return Errors.CustomFieldSetup.NotFound;

        customFieldSetupRepository.Remove(customFieldSetup);
        customFieldSetup.Workspace?.RefreshActivity();
        customFieldSetup.Projects.ForEach(project =>
        {
            customFieldSetup.AddDomainEvent(new RemovedCustomFieldSetupFromProject(customFieldSetup, project));
            project.RefreshActivity();
        });

        return new DeleteCustomFieldSetupResult();
    }
}
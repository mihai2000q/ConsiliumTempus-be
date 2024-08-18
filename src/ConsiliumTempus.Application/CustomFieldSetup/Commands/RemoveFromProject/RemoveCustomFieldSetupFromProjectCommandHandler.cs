using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.CustomFieldSetup.Commands.RemoveFromProject;

public sealed class RemoveCustomFieldSetupFromProjectCommandHandler(
    IProjectRepository projectRepository,
    ICustomFieldSetupRepository customFieldSetupRepository)
    : IRequestHandler<RemoveCustomFieldSetupFromProjectCommand, ErrorOr<RemoveCustomFieldSetupFromProjectResult>>
{
    public async Task<ErrorOr<RemoveCustomFieldSetupFromProjectResult>> Handle(
        RemoveCustomFieldSetupFromProjectCommand command,
        CancellationToken cancellationToken)
    {
        var customFieldSetup = await customFieldSetupRepository.GetWithWorkspaceAndProjects(
            CustomFieldSetupId.Create(command.Id),
            cancellationToken);
        if (customFieldSetup is null) return Errors.CustomFieldSetup.NotFound;
        if (customFieldSetup.Workspace is null) return Errors.CustomFieldSetup.NotGlobal;

        var project = await projectRepository.Get(ProjectId.Create(command.ProjectId), cancellationToken);
        if (project is null) return Errors.Project.NotFound;

        customFieldSetup.RemoveProject(project);
        customFieldSetup.Workspace.RefreshActivity();
        project.RefreshActivity();

        return new RemoveCustomFieldSetupFromProjectResult();
    }
}
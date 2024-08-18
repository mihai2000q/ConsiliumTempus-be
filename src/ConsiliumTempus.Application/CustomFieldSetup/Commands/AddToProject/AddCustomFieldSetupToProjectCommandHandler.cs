using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.CustomFieldSetup.Commands.AddToProject;

public sealed class AddCustomFieldSetupToProjectCommandHandler(
    ICustomFieldSetupRepository customFieldSetupRepository,
    IProjectRepository projectRepository)
    : IRequestHandler<AddCustomFieldSetupToProjectCommand, ErrorOr<AddCustomFieldSetupToProjectResult>>
{
    public async Task<ErrorOr<AddCustomFieldSetupToProjectResult>> Handle(AddCustomFieldSetupToProjectCommand command,
        CancellationToken cancellationToken)
    {
        var customFieldSetup = await customFieldSetupRepository.GetWithWorkspaceAndProjects(
            CustomFieldSetupId.Create(command.Id),
            cancellationToken);
        if (customFieldSetup is null) return Errors.CustomFieldSetup.NotFound;
        if (customFieldSetup.Workspace is null) return Errors.CustomFieldSetup.NotGlobal;

        var project = await projectRepository.Get(ProjectId.Create(command.ProjectId), cancellationToken);
        if (project is null) return Errors.Project.NotFound;
        if (customFieldSetup.Projects.Contains(project)) return Errors.CustomFieldSetup.ProjectAlreadyAdded;

        customFieldSetup.AddProject(project);
        project.RefreshActivity();

        return new AddCustomFieldSetupToProjectResult();
    }
}
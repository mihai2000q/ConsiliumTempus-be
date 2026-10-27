using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project.ValueObjects;

namespace ConsiliumTempus.Domain.Common.Relations;

public sealed class ProjectHasCustomFieldSetup : Entity<(ProjectId, CustomFieldSetupId)>
{
    private ProjectHasCustomFieldSetup() {}

    public override (ProjectId, CustomFieldSetupId) Id => new(ProjectsId, CustomFieldSetupsId);
    public ProjectId ProjectsId { get; init; } = null!;
    public CustomFieldSetupId CustomFieldSetupsId { get; init; } = null!;
}
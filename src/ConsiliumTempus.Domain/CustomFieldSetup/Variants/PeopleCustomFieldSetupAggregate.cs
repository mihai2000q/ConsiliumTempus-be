using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Domain.CustomFieldSetup.Variants;

public sealed class PeopleCustomFieldSetupAggregate : CustomFieldSetupAggregate
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private PeopleCustomFieldSetupAggregate()
    {
    }

    private PeopleCustomFieldSetupAggregate(
        CustomFieldSetupId id,
        Name name,
        Description description,
        WorkspaceAggregate? workspace) : base(id, name, description, workspace)
    {
    }

    public static PeopleCustomFieldSetupAggregate Create(
        Name name,
        Description description,
        WorkspaceAggregate? workspace,
        ProjectAggregate? project)
    {
        var setup = new PeopleCustomFieldSetupAggregate(
            CustomFieldSetupId.CreateUnique(),
            name,
            description,
            workspace);

        if (project != null) setup.AddProject(project);

        return setup;
    }

    public new void Update(
        Name name,
        Description description)
    {
        base.Update(name, description);
    }
}
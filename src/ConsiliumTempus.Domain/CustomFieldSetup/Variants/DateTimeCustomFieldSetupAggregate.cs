using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Domain.CustomFieldSetup.Variants;

public sealed class DateTimeCustomFieldSetupAggregate : CustomFieldSetupAggregate
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private DateTimeCustomFieldSetupAggregate()
    {
    }

    private DateTimeCustomFieldSetupAggregate(
        DateTime? defaultDateTime,
        CustomFieldSetupId id,
        Name name,
        Description description,
        WorkspaceAggregate? workspace) : base(id, name, description, workspace)
    {
        DefaultDateTime = defaultDateTime;
    }

    public DateTime? DefaultDateTime { get; private set; }

    public static DateTimeCustomFieldSetupAggregate Create(
        DateTime? defaultDateTime,
        Name name,
        Description description,
        WorkspaceAggregate? workspace,
        ProjectAggregate? project)
    {
        var setup = new DateTimeCustomFieldSetupAggregate(
            defaultDateTime,
            CustomFieldSetupId.CreateUnique(),
            name,
            description,
            workspace);
        
        if (project != null) setup.AddProject(project);

        return setup;
    }

    public void Update(
        DateTime? defaultDateTime,
        Name name,
        Description description)
    {
        DefaultDateTime = defaultDateTime;
        base.Update(name, description);
    }
}
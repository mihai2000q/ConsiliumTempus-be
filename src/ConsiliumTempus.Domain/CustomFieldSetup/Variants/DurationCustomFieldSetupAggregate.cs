using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Domain.CustomFieldSetup.Variants;

public sealed class DurationCustomFieldSetupAggregate : CustomFieldSetupAggregate
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private DurationCustomFieldSetupAggregate()
    {
    }

    private DurationCustomFieldSetupAggregate(
        TimeSpan? defaultDuration,
        CustomFieldSetupId id,
        Name name,
        Description description,
        WorkspaceAggregate? workspace) : base(id, name, description, workspace)
    {
        DefaultDuration = defaultDuration;
    }

    public TimeSpan? DefaultDuration { get; private set; }

    public override CustomFieldType Type => CustomFieldType.Duration;

    public static DurationCustomFieldSetupAggregate Create(
        TimeSpan? defaultDuration,
        Name name,
        Description description,
        WorkspaceAggregate? workspace,
        ProjectAggregate? project)
    {
        var setup = new DurationCustomFieldSetupAggregate(
            defaultDuration,
            CustomFieldSetupId.CreateUnique(),
            name,
            description,
            workspace);
        
        if (project != null) setup.AddProject(project);

        return setup;
    }

    public void Update(
        TimeSpan? defaultDuration,
        Name name,
        Description description)
    {
        DefaultDuration = defaultDuration;
        base.Update(name, description);
    }
}
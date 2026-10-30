using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Domain.CustomFieldSetup.Variants;

public sealed class NumberCustomFieldSetupAggregate : CustomFieldSetupAggregate
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private NumberCustomFieldSetupAggregate()
    {
    }

    private NumberCustomFieldSetupAggregate(
        NumberCustomFieldSettings settings,
        DecimalNumber? defaultNumber,
        CustomFieldSetupId id,
        Name name,
        Description description,
        WorkspaceAggregate? workspace) : base(id, name, description, workspace)
    {
        Settings = settings;
        DefaultNumber = defaultNumber;
    }

    public NumberCustomFieldSettings Settings { get; private set; } = null!;
    public DecimalNumber? DefaultNumber { get; private set; }

    public static NumberCustomFieldSetupAggregate Create(
        NumberCustomFieldSettings settings,
        DecimalNumber? defaultNumber,
        Name name,
        Description description,
        WorkspaceAggregate? workspace,
        ProjectAggregate? project)
    {
        var setup = new NumberCustomFieldSetupAggregate(
            settings,
            defaultNumber,
            CustomFieldSetupId.CreateUnique(),
            name,
            description,
            workspace);
        
        if (project != null) setup.AddProject(project);

        return setup;
    }

    public void Update(
        NumberCustomFieldSettings settings,
        DecimalNumber? defaultNumber,
        Name name,
        Description description)
    {
        Settings = settings;
        DefaultNumber = defaultNumber;
        base.Update(name, description);
    }
}
using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Domain.CustomFieldSetup.Variants;

public sealed class TextCustomFieldSetupAggregate : CustomFieldSetupAggregate
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private TextCustomFieldSetupAggregate()
    {
    }

    private TextCustomFieldSetupAggregate(
        Text? defaultText,
        CustomFieldSetupId id,
        Name name,
        Description description,
        WorkspaceAggregate? workspace) : base(id, name, description, workspace)
    {
        DefaultText = defaultText;
    }

    public Text? DefaultText { get; private set; }

    public static TextCustomFieldSetupAggregate Create(
        Text? defaultText,
        Name name,
        Description description,
        WorkspaceAggregate? workspace,
        ProjectAggregate? project)
    {
        var setup = new TextCustomFieldSetupAggregate(
            defaultText,
            CustomFieldSetupId.CreateUnique(),
            name,
            description,
            workspace);

        if (project != null) setup.AddProject(project);

        return setup;
    }

    public void Update(
        Text? defaultText,
        Name name,
        Description description)
    {
        DefaultText = defaultText;
        base.Update(name, description);
    }
}
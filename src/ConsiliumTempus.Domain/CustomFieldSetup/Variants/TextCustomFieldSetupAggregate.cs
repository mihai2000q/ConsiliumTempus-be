using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;
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
        WorkspaceAggregate? workspace,
        Audit audit) : base(id, name, description, workspace, audit)
    {
        DefaultText = defaultText;
    }

    public Text? DefaultText { get; private set; }

    public static TextCustomFieldSetupAggregate Create(
        Text? defaultText,
        Name name,
        Description description,
        WorkspaceAggregate? workspace,
        ProjectAggregate? project,
        UserAggregate createdBy)
    {
        var setup = new TextCustomFieldSetupAggregate(
            defaultText,
            CustomFieldSetupId.CreateUnique(),
            name,
            description,
            workspace,
            Audit.Create(createdBy));

        if (project != null) setup.AddProject(project);

        return setup;
    }

    public void Update(
        Text? defaultText,
        Name name,
        Description description,
        UserAggregate updatedBy)
    {
        DefaultText = defaultText;
        base.Update(name, description, updatedBy);
    }
}
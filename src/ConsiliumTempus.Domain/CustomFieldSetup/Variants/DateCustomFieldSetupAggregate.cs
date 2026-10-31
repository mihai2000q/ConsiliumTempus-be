using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Domain.CustomFieldSetup.Variants;

public sealed class DateCustomFieldSetupAggregate : CustomFieldSetupAggregate
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private DateCustomFieldSetupAggregate()
    {
    }

    private DateCustomFieldSetupAggregate(
        DateOnly? defaultDate,
        CustomFieldSetupId id,
        Name name,
        Description description,
        WorkspaceAggregate? workspace) : base(id, name, description, workspace)
    {
        DefaultDate = defaultDate;
    }

    public DateOnly? DefaultDate { get; private set; }

    public override CustomFieldType Type => CustomFieldType.Date;

    public static DateCustomFieldSetupAggregate Create(
        DateOnly? defaultDate,
        Name name,
        Description description,
        WorkspaceAggregate? workspace,
        ProjectAggregate? project)
    {
        var setup = new DateCustomFieldSetupAggregate(
            defaultDate,
            CustomFieldSetupId.CreateUnique(),
            name,
            description,
            workspace);
        
        if (project != null) setup.AddProject(project);

        return setup;
    }

    public void Update(
        DateOnly? defaultDate,
        Name name,
        Description description)
    {
        DefaultDate = defaultDate;
        base.Update(name, description);
    }
}
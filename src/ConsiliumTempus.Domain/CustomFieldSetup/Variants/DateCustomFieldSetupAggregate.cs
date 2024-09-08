using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;
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
        WorkspaceAggregate? workspace,
        Audit audit) : base(id, name, description, workspace, audit)
    {
        DefaultDate = defaultDate;
    }

    public DateOnly? DefaultDate { get; private set; }

    public static DateCustomFieldSetupAggregate Create(
        DateOnly? defaultDate,
        Name name,
        Description description,
        WorkspaceAggregate? workspace,
        ProjectAggregate? project,
        UserAggregate createdBy)
    {
        var setup = new DateCustomFieldSetupAggregate(
            defaultDate,
            CustomFieldSetupId.CreateUnique(),
            name,
            description,
            workspace,
            Audit.Create(createdBy));
        
        if (project != null) setup.AddProject(project);

        return setup;
    }

    public void Update(
        DateOnly? defaultDate,
        Name name,
        Description description,
        UserAggregate updatedBy)
    {
        DefaultDate = defaultDate;
        base.Update(name, description, updatedBy);
    }
}
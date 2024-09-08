using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;
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
        WorkspaceAggregate? workspace,
        Audit audit) : base(id, name, description, workspace, audit)
    {
        DefaultDateTime = defaultDateTime;
    }

    public DateTime? DefaultDateTime { get; private set; }

    public static DateTimeCustomFieldSetupAggregate Create(
        DateTime? defaultDateTime,
        Name name,
        Description description,
        WorkspaceAggregate? workspace,
        ProjectAggregate? project,
        UserAggregate createdBy)
    {
        var setup = new DateTimeCustomFieldSetupAggregate(
            defaultDateTime,
            CustomFieldSetupId.CreateUnique(),
            name,
            description,
            workspace,
            Audit.Create(createdBy));
        
        if (project != null) setup.AddProject(project);

        return setup;
    }

    public void Update(
        DateTime? defaultDateTime,
        Name name,
        Description description,
        UserAggregate updatedBy)
    {
        DefaultDateTime = defaultDateTime;
        base.Update(name, description, updatedBy);
    }
}
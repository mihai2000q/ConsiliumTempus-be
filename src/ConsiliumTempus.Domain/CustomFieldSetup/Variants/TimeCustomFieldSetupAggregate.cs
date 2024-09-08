using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Domain.CustomFieldSetup.Variants;

public sealed class TimeCustomFieldSetupAggregate : CustomFieldSetupAggregate
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private TimeCustomFieldSetupAggregate()
    {
    }

    private TimeCustomFieldSetupAggregate(
        TimeOnly? defaultTime,
        CustomFieldSetupId id,
        Name name,
        Description description,
        WorkspaceAggregate? workspace,
        Audit audit) : base(id, name, description, workspace, audit)
    {
        DefaultTime = defaultTime;
    }

    public TimeOnly? DefaultTime { get; private set; }

    public static TimeCustomFieldSetupAggregate Create(
        TimeOnly? defaultTime,
        Name name,
        Description description,
        WorkspaceAggregate? workspace,
        ProjectAggregate? project,
        UserAggregate createdBy)
    {
        var setup = new TimeCustomFieldSetupAggregate(
            defaultTime,
            CustomFieldSetupId.CreateUnique(),
            name,
            description,
            workspace,
            Audit.Create(createdBy));
        
        if (project != null) setup.AddProject(project);

        return setup;
    }

    public void Update(
        TimeOnly? defaultTime,
        Name name,
        Description description,
        UserAggregate upTimedBy)
    {
        DefaultTime = defaultTime;
        base.Update(name, description, upTimedBy);
    }
}
using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;
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
        WorkspaceAggregate? workspace,
        Audit audit) : base(id, name, description, workspace, audit)
    {
        DefaultDuration = defaultDuration;
    }

    public TimeSpan? DefaultDuration { get; private set; }

    public static DurationCustomFieldSetupAggregate Create(
        TimeSpan? defaultDuration,
        Name name,
        Description description,
        WorkspaceAggregate? workspace,
        ProjectAggregate? project,
        UserAggregate createdBy)
    {
        var setup = new DurationCustomFieldSetupAggregate(
            defaultDuration,
            CustomFieldSetupId.CreateUnique(),
            name,
            description,
            workspace,
            Audit.Create(createdBy));
        
        if (project != null) setup.AddProject(project);

        return setup;
    }

    public void Update(
        TimeSpan? defaultDuration,
        Name name,
        Description description,
        UserAggregate updatedBy)
    {
        DefaultDuration = defaultDuration;
        base.Update(name, description, updatedBy);
    }
}
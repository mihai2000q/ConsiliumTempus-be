using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project;
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
        WorkspaceAggregate? workspace) : base(id, name, description, workspace)
    {
        DefaultTime = defaultTime;
    }

    public TimeOnly? DefaultTime { get; private set; }

    public static TimeCustomFieldSetupAggregate Create(
        TimeOnly? defaultTime,
        Name name,
        Description description,
        WorkspaceAggregate? workspace,
        ProjectAggregate? project)
    {
        var setup = new TimeCustomFieldSetupAggregate(
            defaultTime,
            CustomFieldSetupId.CreateUnique(),
            name,
            description,
            workspace);
        
        if (project != null) setup.AddProject(project);

        return setup;
    }

    public void Update(
        TimeOnly? defaultTime,
        Name name,
        Description description)
    {
        DefaultTime = defaultTime;
        base.Update(name, description);
    }
}
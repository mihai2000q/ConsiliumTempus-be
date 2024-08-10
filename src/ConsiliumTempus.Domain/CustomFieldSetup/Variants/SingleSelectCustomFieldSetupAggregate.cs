using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.Entities;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Domain.CustomFieldSetup.Variants;

public sealed class SingleSelectCustomFieldSetupAggregate : CustomFieldSetupAggregate
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private SingleSelectCustomFieldSetupAggregate()
    {
    }

    private SingleSelectCustomFieldSetupAggregate(
        List<SingleSelectOption> options,
        CustomFieldSetupId id,
        Name name,
        Description description,
        WorkspaceAggregate? workspace,
        ProjectAggregate? project,
        Audit audit) : base(id, name, description, workspace, project, audit)
    {
        Options = options;
    }

    public List<SingleSelectOption> Options { get; private set; } = null!;

    public static SingleSelectCustomFieldSetupAggregate Create(
        List<SingleSelectOption> options,
        Name name,
        Description description,
        WorkspaceAggregate? workspace,
        ProjectAggregate? project,
        UserAggregate createdBy)
    {
        return new SingleSelectCustomFieldSetupAggregate(
            options,
            CustomFieldSetupId.CreateUnique(), 
            name,
            description,  
            workspace, 
            project, 
            Audit.Create(createdBy));
    }

    public void Update(
        List<SingleSelectOption> options,
        Name name,
        Description description,
        UserAggregate updatedBy)
    {
        Options = options;
        base.Update(name, description, updatedBy);
    }
}
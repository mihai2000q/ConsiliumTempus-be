using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.Events;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;
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
        WorkspaceAggregate? workspace,
        ProjectAggregate? project,
        Audit audit) : base(id, name, description, workspace, project, audit)
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
        ProjectAggregate? project,
        UserAggregate createdBy)
    {
        var setup = new NumberCustomFieldSetupAggregate(
            settings,
            defaultNumber,
            CustomFieldSetupId.CreateUnique(),
            name,
            description,
            workspace,
            project,
            Audit.Create(createdBy));

        setup.AddDomainEvent(new CustomFieldSetupCreated(setup));

        return setup;
    }

    public void Update(
        NumberCustomFieldSettings settings,
        DecimalNumber? defaultNumber,
        Name name,
        Description description,
        UserAggregate updatedBy)
    {
        Settings = settings;
        DefaultNumber = defaultNumber;
        base.Update(name, description, updatedBy);
    }
}
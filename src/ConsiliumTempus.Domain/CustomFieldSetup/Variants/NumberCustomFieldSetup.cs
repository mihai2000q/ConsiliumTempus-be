using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Domain.CustomFieldSetup.Variants;

public sealed class NumberCustomFieldSetup : CustomFieldSetupAggregate
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private NumberCustomFieldSetup()
    {
    }

    private NumberCustomFieldSetup(
        NumberCustomFieldSettings settings,
        CustomFieldSetupId id,
        Name name,
        Description description,
        WorkspaceAggregate? workspace,
        ProjectAggregate? project,
        Audit audit) : base(id, name, description, workspace, project, audit)
    {
        Settings = settings;
    }

    public NumberCustomFieldSettings Settings { get; init; } = null!;

    public static NumberCustomFieldSetup Create(
        NumberCustomFieldSettings settings,
        Name name,
        Description description,
        WorkspaceAggregate? workspace,
        ProjectAggregate? project,
        UserAggregate createdBy)
    {
        return new NumberCustomFieldSetup(
            settings,
            CustomFieldSetupId.CreateUnique(),
            name,
            description,
            workspace,
            project,
            Audit.Create(createdBy));
    }
}
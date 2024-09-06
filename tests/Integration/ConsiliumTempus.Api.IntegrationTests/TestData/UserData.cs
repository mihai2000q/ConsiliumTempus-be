using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Common.IntegrationTests.Common.Entities;
using ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup;
using ConsiliumTempus.Common.IntegrationTests.Project;
using ConsiliumTempus.Common.IntegrationTests.ProjectSprint;
using ConsiliumTempus.Common.IntegrationTests.ProjectSprint.Entities;
using ConsiliumTempus.Common.IntegrationTests.ProjectTask;
using ConsiliumTempus.Common.IntegrationTests.ProjectTask.Entities;
using ConsiliumTempus.Common.IntegrationTests.User;
using ConsiliumTempus.Common.IntegrationTests.Workspace;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Api.IntegrationTests.TestData;

internal class UserData : ITestData
{
    public IEnumerable<IEnumerable<object>> GetDataCollections()
    {
        return
        [
            Users,
            Workspaces,
            Memberships,
            Projects,
            CustomFieldSetups,
            ProjectSprints,
            ProjectStages,
            ProjectTasks,
            CustomFields
        ];
    }

    public static readonly UserAggregate[] Users =
    [
        UserFactory.Create(
            "michaelj@gmail.com",
            "pass",
            "Michael",
            "Jordan",
            "Pro Basketball Player",
            new DateOnly(2000, 12, 23)),
        UserFactory.Create(
            "leom@gmail.com",
            "pass",
            "Leo",
            "Messi"),
        UserFactory.Create(
            "cristianor@gmail.com",
            "pass",
            "Cristiano",
            "Ronaldo"),
        UserFactory.Create(
            "stephenc@gmail.com",
            "pass",
            "Stephen",
            "Curry"),
        UserFactory.Create(
            "lebronj@gmail.com",
            "pass",
            "Lebron",
            "James"),
        UserFactory.Create(
            "kevind@gmail.com",
            "pass",
            "Kevin",
            "Durant"),
    ];

    public static readonly WorkspaceAggregate[] Workspaces =
    [
        WorkspaceFactory.Create(
            Users[0],
            "Basketball",
            "This is the Description of the first Workspace"),
        WorkspaceFactory.Create(
            Users[1],
            "Football",
            "This is the Description of the second Workspace"),
        WorkspaceFactory.Create(
            Users[0],
            "Michael Group",
            "This is the Description of the third Workspace",
            true),
        WorkspaceFactory.Create(
            Users[0],
            "Michael and Lebron are best friends <3",
            "",
            true), // normally not possible, but testing out multiple use cases at once
    ];

    public static readonly Membership[] Memberships =
    [
        MembershipFactory.Create(
            Users[0],
            Workspaces[0],
            WorkspaceRole.Admin),
        MembershipFactory.Create(
            Users[0],
            Workspaces[2],
            WorkspaceRole.Admin),
        MembershipFactory.Create(
            Users[0],
            Workspaces[3],
            WorkspaceRole.Admin),
        MembershipFactory.Create(
            Users[1],
            Workspaces[1],
            WorkspaceRole.Admin),
        MembershipFactory.Create(
            Users[2],
            Workspaces[1],
            WorkspaceRole.Member),
        MembershipFactory.Create(
            Users[3],
            Workspaces[0],
            WorkspaceRole.Member),
        MembershipFactory.Create(
            Users[4],
            Workspaces[0],
            WorkspaceRole.View),
        MembershipFactory.Create(
            Users[4],
            Workspaces[3],
            WorkspaceRole.Member),
        MembershipFactory.Create(
            Users[5],
            Workspaces[0],
            WorkspaceRole.Admin)
    ];

    public static readonly ProjectAggregate[] Projects =
    [
        ProjectFactory.Create(
            Workspaces[0],
            Users[0],
            "Win NBA - with new owner",
            "Plan to win NBA"),
        ProjectFactory.Create(
            Workspaces[0],
            Users[0],
            name: "project will be deleted",
            isPrivate: true,
            allowedMembers: [Users[0]]),
        ProjectFactory.Create(
            Workspaces[2],
            Users[0],
            name: "Workspace will be deleted",
            isPrivate: true,
            allowedMembers: [Users[0]]),
        ProjectFactory.Create(
            Workspaces[3],
            Users[0],
            name: "new owner"),
        ProjectFactory.Create(
            Workspaces[3],
            Users[0],
            name: "deleted",
            isPrivate: true,
            allowedMembers: [Users[0]])
    ];

    public static readonly ProjectSprintAggregate[] ProjectSprints =
    [
        ProjectSprintFactory.Create(
            Projects[0],
            Audit.Create(Users[0]),
            "Sprint 1 - Qualify on Semi Finals",
            new DateOnly(2024, 01, 16),
            new DateOnly(2024, 01, 30)),
        ProjectSprintFactory.Create(
            Projects[1],
            Audit.Create(Users[0])),
    ];

    private static readonly SingleSelectOption[] SingleSelectOptions =
    [
        SingleSelectOptionFactory.Create(),
        SingleSelectOptionFactory.Create(customOrderPosition: 1),

        SingleSelectOptionFactory.Create(),
        SingleSelectOptionFactory.Create(customOrderPosition: 1),
    ];

    public static readonly CustomFieldSetupAggregate[] CustomFieldSetups =
    [
        CustomFieldSetupFactory.CreateNumber(
            null,
            [Projects[0]],
            AuditFactory.Create(Users[0]),
            "A number field"),
        CustomFieldSetupFactory.CreateSingleSelect(
            null,
            [Projects[0]],
            AuditFactory.Create(Users[0]),
            [
                SingleSelectOptions[0],
                SingleSelectOptions[1]
            ],
            "Select only one field",
            defaultOption: SingleSelectOptions[0]),
        CustomFieldSetupFactory.CreateText(
            null,
            [Projects[0]],
            AuditFactory.Create(Users[0]),
            "Notes field"),

        CustomFieldSetupFactory.CreateNumber(
            null,
            [Projects[1]],
            AuditFactory.Create(Users[0]),
            "A number field"),
        CustomFieldSetupFactory.CreateSingleSelect(
            null,
            [Projects[1]],
            AuditFactory.Create(Users[0]),
            [
                SingleSelectOptions[2],
                SingleSelectOptions[3]
            ],
            "Select only one field",
            defaultOption: SingleSelectOptions[2]),
        CustomFieldSetupFactory.CreateText(
            null,
            [Projects[1]],
            AuditFactory.Create(Users[0]),
            "Notes field")
    ];

    public static readonly ProjectStage[] ProjectStages =
    [
        ProjectStageFactory.Create(
            ProjectSprints[0],
            AuditFactory.Create(Users[0]),
            "To do"),

        ProjectStageFactory.Create(
            ProjectSprints[1],
            AuditFactory.Create(Users[0]),
            "To do2"),
    ];

    public static readonly ProjectTaskAggregate[] ProjectTasks =
    [
        ProjectTaskFactory.Create(
            Users[1], // TODO: Make it back to Users[0], but for the moment the Cascade removes it, which shouldn't happen as the tasks should be preserved
            ProjectStages[0],
            "Should do more dribbling"),

        ProjectTaskFactory.Create(
            Users[1],
            ProjectStages[1]),
    ];

    public static readonly CustomField[] CustomFields =
    [
        CustomFieldFactory.CreateNumber(
            ProjectTasks[0],
            (NumberCustomFieldSetupAggregate)CustomFieldSetups[0],
            500),
        CustomFieldFactory.CreateSingleSelect(
            ProjectTasks[0],
            (SingleSelectCustomFieldSetupAggregate)CustomFieldSetups[1],
            ((SingleSelectCustomFieldSetupAggregate)CustomFieldSetups[1]).Options[0]),
        CustomFieldFactory.CreateText(
            ProjectTasks[0],
            (TextCustomFieldSetupAggregate)CustomFieldSetups[2],
            "Something"),

        CustomFieldFactory.CreateNumber(
            ProjectTasks[1],
            (NumberCustomFieldSetupAggregate)CustomFieldSetups[^3]),
        CustomFieldFactory.CreateSingleSelect(
            ProjectTasks[1],
            (SingleSelectCustomFieldSetupAggregate)CustomFieldSetups[^2],
            ((SingleSelectCustomFieldSetupAggregate)CustomFieldSetups[^2]).Options[0]),
        CustomFieldFactory.CreateText(
            ProjectTasks[1],
            (TextCustomFieldSetupAggregate)CustomFieldSetups[^1]),
    ];
}
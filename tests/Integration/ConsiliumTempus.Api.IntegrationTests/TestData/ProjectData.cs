using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Common.IntegrationTests.Common.Entities;
using ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup;
using ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup.Entities;
using ConsiliumTempus.Common.IntegrationTests.Project;
using ConsiliumTempus.Common.IntegrationTests.Project.Entities;
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
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.Enums;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Api.IntegrationTests.TestData;

internal class ProjectData : ITestData
{
    public IEnumerable<IEnumerable<object>> GetDataCollections()
    {
        return
        [
            Users,
            Workspaces,
            Memberships,
            Projects,
            Statuses,
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
            Users[3],
            Workspaces[2],
            WorkspaceRole.Admin),
        MembershipFactory.Create(
            Users[4],
            Workspaces[0],
            WorkspaceRole.View),
        MembershipFactory.Create(
            Users[4],
            Workspaces[2],
            WorkspaceRole.Admin)
    ];

    public static readonly ProjectAggregate[] Projects =
    [
        ProjectFactory.Create(
            Workspaces[0],
            Users[0],
            "Win NBA",
            "This is an elaborate plan to win NBA",
            favorites: [Users[0]],
            allowedMembers: [Users[0], Users[3]]),
        ProjectFactory.Create(
            Workspaces[1],
            Users[1],
            "Win Champions League",
            "Just an idea on how to win the football league",
            favorites: [Users[1]]),
        ProjectFactory.Create(
            Workspaces[0],
            Users[0],
            "Win Another Tournament"),
        ProjectFactory.Create(
            Workspaces[2],
            Users[0],
            "Get a house",
            "this is actually really important",
            favorites: [Users[0]]),
        ProjectFactory.Create(
            Workspaces[2],
            Users[0],
            "Start a retiring investment plan",
            "Should do it as soon as possible",
            false,
            ProjectLifecycle.Upcoming,
            allowedMembers: [Users[0]]),
        ProjectFactory.Create(
            Workspaces[2],
            Users[0],
            "Start a new life",
            "criminal activity",
            false,
            ProjectLifecycle.Archived,
            favorites: [Users[0]]),

        ProjectFactory.Create(
            Workspaces[2],
            Users[1],
            "Something Private - owner not anymore",
            description: "but the owner used to be a collaborator, however this was fixed already",
            allowedMembers: [Users[1], Users[0]]),

        ProjectFactory.Create(
            Workspaces[2],
            Users[0],
            "Something Not Private",
            isPrivate: false,
            allowedMembers: [Users[0]]),
        ProjectFactory.Create(
            Workspaces[2],
            Users[0],
            "Something Private",
            isPrivate: true,
            allowedMembers: [Users[0], Users[3]],
            favorites: [Users[3]]),
        ProjectFactory.Create(
            Workspaces[2],
            Users[3],
            "Something More Private",
            isPrivate: true,
            allowedMembers: [Users[3]]),
    ];

    public static readonly ProjectStatus[] Statuses =
    [
        ProjectStatusFactory.Create(
            Projects[0],
            AuditFactory.Create(Users[0]),
            "This is a status update"),
        ProjectStatusFactory.Create(
            Projects[0],
            AuditFactory.Create(Users[0]),
            "Another status update 0",
            "Project is off track officially... training is too hard",
            ProjectStatusType.OffTrack),

        ProjectStatusFactory.Create(
            Projects[^3],
            AuditFactory.Create(Users[0]),
            "Another status update 1"),
        ProjectStatusFactory.Create(
            Projects[^2],
            AuditFactory.Create(Users[0]),
            "Another status update 2"),
        ProjectStatusFactory.Create(
            Projects[^1],
            AuditFactory.Create(Users[0]),
            "Another status update 3"),
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
                SingleSelectOptionFactory.Create(),
                SingleSelectOptionFactory.Create(customOrderPosition: 1),
            ],
            "Select only one field"),
        CustomFieldSetupFactory.CreateText(
            null,
            [Projects[0]],
            AuditFactory.Create(Users[0]),
            "Notes field")
    ];

    public static readonly ProjectSprintAggregate[] ProjectSprints =
    [
        ProjectSprintFactory.Create(
            Projects[0],
            Audit.Create(Users[0]),
            "Sprint 1 - Qualify on Semi Finals",
            new DateOnly(2024, 01, 16),
            new DateOnly(2024, 01, 30)),
    ];

    public static readonly ProjectStage[] ProjectStages =
    [
        ProjectStageFactory.Create(
            ProjectSprints[0],
            AuditFactory.Create(Users[0]),
            "To do"),
        ProjectStageFactory.Create(
            ProjectSprints[0],
            AuditFactory.Create(Users[0]),
            "In Progress",
            1),
        ProjectStageFactory.Create(
            ProjectSprints[0],
            AuditFactory.Create(Users[0]),
            "Done",
            2),
    ];

    public static readonly ProjectTaskAggregate[] ProjectTasks =
    [
        ProjectTaskFactory.Create(
            Users[0],
            ProjectStages[0],
            "Should do more dribbling"),
        ProjectTaskFactory.Create(
            Users[0],
            ProjectStages[0],
            "Should add more stepping to my shots",
            customOrderPosition: 1,
            assignee: Users[1],
            isCompleted: true),
        ProjectTaskFactory.Create(
            Users[3],
            ProjectStages[0],
            "Should tell Michael to PASS MOORE!!",
            assignee: Users[0],
            customOrderPosition: 2),
        ProjectTaskFactory.Create(
            Users[3],
            ProjectStages[0],
            "Tell Michael to DRIBBLE LESS!!",
            assignee: Users[0],
            customOrderPosition: 3),

        ProjectTaskFactory.Create(
            Users[0],
            ProjectStages[1],
            "We want to win the cup"),
        ProjectTaskFactory.Create(
            Users[0],
            ProjectStages[1],
            "We want to win them all",
            customOrderPosition: 1),
        ProjectTaskFactory.Create(
            Users[0],
            ProjectStages[1],
            "We want to go to coffee after",
            customOrderPosition: 2),
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
            (NumberCustomFieldSetupAggregate)CustomFieldSetups[0]),
        CustomFieldFactory.CreateSingleSelect(
            ProjectTasks[1],
            (SingleSelectCustomFieldSetupAggregate)CustomFieldSetups[1]),
        CustomFieldFactory.CreateText(
            ProjectTasks[1],
            (TextCustomFieldSetupAggregate)CustomFieldSetups[2]),
    ];
}
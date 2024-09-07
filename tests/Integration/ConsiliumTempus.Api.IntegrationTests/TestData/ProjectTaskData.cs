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

internal class ProjectTaskData : ITestData
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
            "This is an elaborate plan to win NBA"),

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
            allowedMembers: [Users[0], Users[3]]),
        ProjectFactory.Create(
            Workspaces[2],
            Users[3],
            "Something More Private",
            isPrivate: true,
            allowedMembers: [Users[3]]),
    ];

    public static readonly CustomFieldSetupAggregate[] CustomFieldSetups =
    [
        CustomFieldSetupFactory.CreateDate(
            null,
            [Projects[0]],
            AuditFactory.Create(Users[0]),
            "A date field"),
        CustomFieldSetupFactory.CreateDateTime(
            null,
            [Projects[0]],
            AuditFactory.Create(Users[0]),
            "A date time field"),
        CustomFieldSetupFactory.CreateDuration(
            null,
            [Projects[0]],
            AuditFactory.Create(Users[0]),
            "A duration field"),
        CustomFieldSetupFactory.CreateMultiSelect(
            null,
            [Projects[0]],
            AuditFactory.Create(Users[0]),
            [
                MultiSelectOptionFactory.Create(),
                MultiSelectOptionFactory.Create(customOrderPosition: 1),
            ],
            "Select multiple fields"),
        CustomFieldSetupFactory.CreateNumber(
            null,
            [Projects[0]],
            AuditFactory.Create(Users[0]),
            "A number field"),
        CustomFieldSetupFactory.CreatePeople(
            null,
            [Projects[0]],
            AuditFactory.Create(Users[0]),
            "A People field"),
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
            name: "A Text Field"),
        CustomFieldSetupFactory.CreateTime(
            null,
            [Projects[0]],
            AuditFactory.Create(Users[0]),
            "A Time field"),
        
        CustomFieldSetupFactory.CreateNumber(
            null,
            [Projects[^3]],
            AuditFactory.Create(Users[0])),
        CustomFieldSetupFactory.CreateNumber(
            null,
            [Projects[^2]],
            AuditFactory.Create(Users[0])),
        CustomFieldSetupFactory.CreateNumber(
            null,
            [Projects[^1]],
            AuditFactory.Create(Users[0])),
    ];

    public static readonly ProjectSprintAggregate[] ProjectSprints =
    [
        ProjectSprintFactory.Create(
            Projects[0],
            Audit.Create(Users[0]),
            "Sprint 1 - Qualify on Quarters",
            new DateOnly(2024, 01, 1),
            new DateOnly(2024, 01, 15)),

        ProjectSprintFactory.Create(
            Projects[1],
            AuditFactory.Create(Users[0]),
            "Not Private Project Sprint"),
        ProjectSprintFactory.Create(
            Projects[2],
            AuditFactory.Create(Users[0]),
            "Private Project Sprint"),
        ProjectSprintFactory.Create(
            Projects[3],
            AuditFactory.Create(Users[3]),
            "More Private Project Sprint"),
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

        ProjectStageFactory.Create(
            ProjectSprints[1],
            AuditFactory.Create(Users[0]),
            "To do"),
        ProjectStageFactory.Create(
            ProjectSprints[2],
            AuditFactory.Create(Users[0]),
            "To do"),
        ProjectStageFactory.Create(
            ProjectSprints[3],
            AuditFactory.Create(Users[3]),
            "To do"),
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

        ProjectTaskFactory.Create(
            Users[0],
            ProjectStages[^3],
            "Not Private Task"),
        ProjectTaskFactory.Create(
            Users[0],
            ProjectStages[^2],
            "Private Task"),
        ProjectTaskFactory.Create(
            Users[3],
            ProjectStages[^1],
            "More Private Task"),

        ProjectTaskFactory.Create(
            Users[0],
            ProjectStages[^3],
            "Not Private Task 2",
            customOrderPosition: 2),
        ProjectTaskFactory.Create(
            Users[0],
            ProjectStages[^2],
            "Private Task",
            customOrderPosition: 2),
        ProjectTaskFactory.Create(
            Users[3],
            ProjectStages[^1],
            "More Private Task 2",
            customOrderPosition: 2),
    ];

    public static readonly CustomField[] CustomFields =
    [
        CustomFieldFactory.CreateDate(
            ProjectTasks[0],
            (DateCustomFieldSetupAggregate)CustomFieldSetups[0],
            new DateOnly(2022, 10, 10)),
        CustomFieldFactory.CreateDateTime(
            ProjectTasks[0],
            (DateTimeCustomFieldSetupAggregate)CustomFieldSetups[1],
            new DateTime(2022, 10, 10, 10, 55, 30)),
        CustomFieldFactory.CreateDuration(
            ProjectTasks[0],
            (DurationCustomFieldSetupAggregate)CustomFieldSetups[2],
            new TimeSpan(10, 55, 30)),
        CustomFieldFactory.CreateMultiSelect(
            ProjectTasks[0],
            (MultiSelectCustomFieldSetupAggregate)CustomFieldSetups[3],
            [
                ((MultiSelectCustomFieldSetupAggregate)CustomFieldSetups[3]).Options[0]
            ]),
        CustomFieldFactory.CreateNumber(
            ProjectTasks[0],
            (NumberCustomFieldSetupAggregate)CustomFieldSetups[4],
            500),
        CustomFieldFactory.CreatePeople(
            ProjectTasks[0],
            (PeopleCustomFieldSetupAggregate)CustomFieldSetups[5],
            Users[3]),
        CustomFieldFactory.CreateSingleSelect(
            ProjectTasks[0],
            (SingleSelectCustomFieldSetupAggregate)CustomFieldSetups[6],
            ((SingleSelectCustomFieldSetupAggregate)CustomFieldSetups[6]).Options[0]),
        CustomFieldFactory.CreateText(
            ProjectTasks[0],
            (TextCustomFieldSetupAggregate)CustomFieldSetups[7],
            "Something"),
        CustomFieldFactory.CreateTime(
            ProjectTasks[0],
            (TimeCustomFieldSetupAggregate)CustomFieldSetups[8],
            new TimeOnly(10, 55)),

        CustomFieldFactory.CreateDate(
            ProjectTasks[1],
            (DateCustomFieldSetupAggregate)CustomFieldSetups[0]),
        CustomFieldFactory.CreateDateTime(
            ProjectTasks[1],
            (DateTimeCustomFieldSetupAggregate)CustomFieldSetups[1]),
        CustomFieldFactory.CreateDuration(
            ProjectTasks[1],
            (DurationCustomFieldSetupAggregate)CustomFieldSetups[2]),
        CustomFieldFactory.CreateMultiSelect(
            ProjectTasks[1],
            (MultiSelectCustomFieldSetupAggregate)CustomFieldSetups[3]),
        CustomFieldFactory.CreateNumber(
            ProjectTasks[1],
            (NumberCustomFieldSetupAggregate)CustomFieldSetups[4]),
        CustomFieldFactory.CreatePeople(
            ProjectTasks[1],
            (PeopleCustomFieldSetupAggregate)CustomFieldSetups[5]),
        CustomFieldFactory.CreateSingleSelect(
            ProjectTasks[1],
            (SingleSelectCustomFieldSetupAggregate)CustomFieldSetups[6]),
        CustomFieldFactory.CreateText(
            ProjectTasks[1],
            (TextCustomFieldSetupAggregate)CustomFieldSetups[7]),
        CustomFieldFactory.CreateTime(
            ProjectTasks[1],
            (TimeCustomFieldSetupAggregate)CustomFieldSetups[8]),
        
        CustomFieldFactory.CreateNumber(
            ProjectTasks[^3],
            (NumberCustomFieldSetupAggregate)CustomFieldSetups[^3]),
        CustomFieldFactory.CreateNumber(
            ProjectTasks[^2],
            (NumberCustomFieldSetupAggregate)CustomFieldSetups[^2]),
        CustomFieldFactory.CreateNumber(
            ProjectTasks[^1],
            (NumberCustomFieldSetupAggregate)CustomFieldSetups[^1]),
    ];
}
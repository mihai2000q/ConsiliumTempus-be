using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Common.IntegrationTests.Common.Entities;
using ConsiliumTempus.Common.IntegrationTests.Project;
using ConsiliumTempus.Common.IntegrationTests.ProjectSprint;
using ConsiliumTempus.Common.IntegrationTests.ProjectSprint.Entities;
using ConsiliumTempus.Common.IntegrationTests.ProjectTask;
using ConsiliumTempus.Common.IntegrationTests.User;
using ConsiliumTempus.Common.IntegrationTests.Workspace;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectTask;
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
            ProjectSprints,
            ProjectStages,
            ProjectTasks
        ];
    }

    public static UserAggregate[] Users { get; } =
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

    public static WorkspaceAggregate[] Workspaces { get; } =
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

    public static Membership[] Memberships { get; } =
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

    public static ProjectAggregate[] Projects { get; } =
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

    public static ProjectSprintAggregate[] ProjectSprints { get; } =
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

    public static ProjectStage[] ProjectStages { get; } =
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

    public static ProjectTaskAggregate[] ProjectTasks { get; } =
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
}
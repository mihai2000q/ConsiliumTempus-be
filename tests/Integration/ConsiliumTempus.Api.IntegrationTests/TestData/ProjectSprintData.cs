using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Common.IntegrationTests.Common.Entities;
using ConsiliumTempus.Common.IntegrationTests.Project;
using ConsiliumTempus.Common.IntegrationTests.ProjectSprint;
using ConsiliumTempus.Common.IntegrationTests.ProjectSprint.Entities;
using ConsiliumTempus.Common.IntegrationTests.User;
using ConsiliumTempus.Common.IntegrationTests.Workspace;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Api.IntegrationTests.TestData;

internal class ProjectSprintData : ITestData
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
            ProjectStages
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

    public static ProjectAggregate[] Projects { get; } =
    [
        ProjectFactory.Create(
            Workspaces[0],
            Users[0],
            "Win NBA",
            "This is an elaborate plan to win NBA"),
        ProjectFactory.Create(
            Workspaces[1],
            Users[1],
            "Win Champions League",
            "Just an idea on how to win the football league"),
        ProjectFactory.Create(
            Workspaces[0],
            Users[0],
            "Do Something with your life?"),
        ProjectFactory.Create(
            Workspaces[0],
            Users[0],
            "Buy a house!"),
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
            AuditFactory.Create(
                Users[0],
                Users[0],
                new DateTime(DateTime.UtcNow.Year, 1, 16, 0, 0, 0, DateTimeKind.Utc)),
            "Sprint 2 - Qualify on Semi Finals",
            new DateOnly(DateTime.UtcNow.Year, 01, 16),
            new DateOnly(DateTime.UtcNow.Year, 01, 30)),
        ProjectSprintFactory.Create(
            Projects[0],
            AuditFactory.Create(
                Users[0],
                Users[0],
                new DateTime(DateTime.UtcNow.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
            "Sprint 1 - Qualify on Quarters",
            new DateOnly(DateTime.UtcNow.Year, 01, 1),
            new DateOnly(DateTime.UtcNow.Year, 01, 15)),
        ProjectSprintFactory.Create(
            Projects[0],
            AuditFactory.Create(
                Users[0],
                Users[0],
                new DateTime(DateTime.UtcNow.Year - 1, 11, 2, 0, 0, 0, DateTimeKind.Utc)),
            "Sprint 0 - Training",
            new DateOnly(DateTime.UtcNow.Year - 1, 11, 22)),
        ProjectSprintFactory.Create(
            Projects[1],
            AuditFactory.Create(Users[1]),
            "Get the champs lig finals",
            new DateOnly(DateTime.UtcNow.Year, 01, 16)),
        ProjectSprintFactory.Create(
            Projects[3],
            AuditFactory.Create(Users[0]),
            "Sprint From Last Year",
            new DateOnly(DateTime.UtcNow.Year - 1, 11, 22)),

        ProjectSprintFactory.Create(
            Projects[4],
            AuditFactory.Create(Users[0]),
            "sprint1"),
        ProjectSprintFactory.Create(
            Projects[5],
            AuditFactory.Create(Users[0]),
            "sprint2"),
        ProjectSprintFactory.Create(
            Projects[6],
            AuditFactory.Create(Users[3]),
            "sprint3"),
        
        ProjectSprintFactory.Create(
            Projects[4],
            AuditFactory.Create(Users[0]),
            "Not Private Project Sprint"),
        ProjectSprintFactory.Create(
            Projects[5],
            AuditFactory.Create(Users[0]),
            "Private Project Sprint"),
        ProjectSprintFactory.Create(
            Projects[6],
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
            "Single Stage"),

        ProjectStageFactory.Create(
            ProjectSprints[2],
            AuditFactory.Create(Users[1]),
            "To do"),
        ProjectStageFactory.Create(
            ProjectSprints[2],
            AuditFactory.Create(Users[1]),
            "In Progress",
            1),
        ProjectStageFactory.Create(
            ProjectSprints[2],
            AuditFactory.Create(Users[1]),
            "Done",
            2),

        ProjectStageFactory.Create(
            ProjectSprints[^3],
            AuditFactory.Create(Users[0]),
            "Stage 1"),
        ProjectStageFactory.Create(
            ProjectSprints[^3],
            AuditFactory.Create(Users[0]),
            "Stage 2",
            1),

        ProjectStageFactory.Create(
            ProjectSprints[^2],
            AuditFactory.Create(Users[0]),
            "Stage 1"),
        ProjectStageFactory.Create(
            ProjectSprints[^2],
            AuditFactory.Create(Users[0]),
            "Stage 2",
            1),

        ProjectStageFactory.Create(
            ProjectSprints[^1],
            AuditFactory.Create(Users[0]),
            "Stage 1"),
        ProjectStageFactory.Create(
            ProjectSprints[^1],
            AuditFactory.Create(Users[0]),
            "Stage 2",
            1),
    ];
}
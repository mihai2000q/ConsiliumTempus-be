using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Common.IntegrationTests.Common.Entities;
using ConsiliumTempus.Common.IntegrationTests.User;
using ConsiliumTempus.Common.IntegrationTests.Workspace;
using ConsiliumTempus.Common.IntegrationTests.Workspace.Entities;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.Entities;

namespace ConsiliumTempus.Api.IntegrationTests.TestData;

internal class WorkspaceData : ITestData
{
    public IEnumerable<IEnumerable<object>> GetDataCollections()
    {
        return
        [
            Users,
            Workspaces,
            Memberships,
            WorkspaceInvitations
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
        UserFactory.Create(
            "johansenm@gmail.com",
            "pass",
            "Johansen",
            "Michael"),
        UserFactory.Create(
            "bsmith@gmail.com",
            "pass",
            "Benjamin",
            "Smith"),
        UserFactory.Create(
            "sfranklin@gmail.com",
            "pass",
            "Samuel",
            "Franklin")
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
        WorkspaceFactory.Create(
            Users[0],
            "Some Group"),
        WorkspaceFactory.Create(
            Users[0],
            "Some Group That I Want To Delete"),
        WorkspaceFactory.Create(
            Users[0],
            "Test Test",
            favorites: [Users[0]]),
        WorkspaceFactory.Create(
            Users[0],
            "Hi There")
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
            Users[5],
            Workspaces[0],
            WorkspaceRole.View),
        MembershipFactory.Create(
            Users[0],
            Workspaces[3],
            WorkspaceRole.Admin),
        MembershipFactory.Create(
            Users[0],
            Workspaces[4],
            WorkspaceRole.Admin),
        MembershipFactory.Create(
            Users[0],
            Workspaces[5],
            WorkspaceRole.Admin),
        MembershipFactory.Create(
            Users[0],
            Workspaces[6],
            WorkspaceRole.Admin),
    ];

    public static WorkspaceInvitation[] WorkspaceInvitations =
    [
        WorkspaceInvitationFactory.Create(
            Users[0],
            Users[7],
            Workspaces[0]), 
    ];
}
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Common.IntegrationTests.Common.Entities;
using ConsiliumTempus.Common.IntegrationTests.Project;
using ConsiliumTempus.Common.IntegrationTests.User;
using ConsiliumTempus.Common.IntegrationTests.Workspace;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Project;
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
            Projects
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
            WorkspaceRole.View)
    ];

    public static ProjectAggregate[] Projects { get; } =
    [
        ProjectFactory.Create(
            Workspaces[0],
            "Win NBA",
            "This is an elaborate plan to win NBA",
            true),
        ProjectFactory.Create(
            Workspaces[1],
            "Win Champions League",
            "Just an idea on how to win the football league",
            true),
        ProjectFactory.Create(
            Workspaces[0],
            "Win Another Tournament"),
        ProjectFactory.Create(
            Workspaces[2],
            "Get a house",
            "this is actually really important",
            false,
            true),
        ProjectFactory.Create(
            Workspaces[2],
            "Get a house",
            "this is actually really important",
            false,
            true),
        ProjectFactory.Create(
            Workspaces[2],
            "Start a new life",
            "criminal activity",
            false,
            true)
    ];
}
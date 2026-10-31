using ConsiliumTempus.ArchitectureTests.TestUtils;
using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.Infrastructure.Security.Authorization.Permission;
using ConsiliumTempus.Infrastructure.Security.Authorization.ProjectAuthorization;
using ConsiliumTempus.Infrastructure.Security.Authorization.Targets;
using ConsiliumTempus.Infrastructure.Security.Authorization.WorkspaceAuthorization;

namespace ConsiliumTempus.ArchitectureTests.Security;

/// <summary>
/// The authorization finds the entity targeted by an endpoint by convention, so, an endpoint that does not follow it
/// (and is not one of the exceptions) would be checked against an id that the model binding does not use,
/// which is a gap in the authorization. This is where such a gap is caught: every authorized endpoint has to be
/// resolved to the very property, in the very place, where its request contract binds the id.
/// </summary>
public class AuthorizationTargetsTest
{
    [Fact]
    public async Task AuthorizationTargets_Should_FindTheAuthorizedEndpoints()
    {
        // Act
        var endpoints = Utils.Security.GetEndpoints();
        var withPermission = await Utils.Security.GetEndpointsRequiring<PermissionRequirement>();
        var withWorkspaceAuthorization = await Utils.Security.GetEndpointsRequiring<WorkspaceAuthorizationRequirement>();
        var withProjectAuthorization = await Utils.Security.GetEndpointsRequiring<ProjectAuthorizationRequirement>();

        // Assert
        endpoints.Should().NotBeEmpty();
        withPermission.Should().NotBeEmpty();
        withWorkspaceAuthorization.Should().NotBeEmpty();
        withProjectAuthorization.Should().NotBeEmpty()
            .And.Contain(e => e.Controller == Utils.Security.GetControllerName(typeof(ProjectSprintController)),
                "the attribute can be on the controller itself");
    }

    [Fact]
    public async Task AuthorizationTargets_Should_ResolveEveryEndpointWithPermissionToItsBinding()
    {
        // Arrange
        var endpoints = await Utils.Security.GetEndpointsRequiring<PermissionRequirement>();

        // Act
        var mismatches = Utils.Security.GetMismatches(endpoints, AuthorizationTargets.For);

        // Assert
        mismatches.Should().BeEmpty();
    }

    [Fact]
    public async Task AuthorizationTargets_Should_ResolveEveryEndpointWithWorkspaceAuthorizationToItsBinding()
    {
        // Arrange
        var endpoints = await Utils.Security.GetEndpointsRequiring<WorkspaceAuthorizationRequirement>();

        // Act
        var mismatches = Utils.Security.GetMismatches(
            endpoints,
            AuthorizationTargets.For,
            [TargetKind.Workspace, TargetKind.Project]);

        // Assert
        mismatches.Should().BeEmpty();
    }

    [Fact]
    public async Task AuthorizationTargets_Should_ResolveEveryEndpointWithProjectAuthorizationToItsBinding()
    {
        // Arrange
        var endpoints = await Utils.Security.GetEndpointsRequiring<ProjectAuthorizationRequirement>();

        // Act
        var mismatches = Utils.Security.GetMismatches(
            endpoints,
            AuthorizationTargets.ForProject,
            [
                TargetKind.CustomFieldSetup, TargetKind.Project, TargetKind.ProjectSprint,
                TargetKind.ProjectStage, TargetKind.ProjectTask
            ]);

        // Assert
        mismatches.Should().BeEmpty();
    }
}

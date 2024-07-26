using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Infrastructure.Security.Authorization.Permission;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using ConsiliumTempus.Infrastructure.Security.Authorization.WorkspaceAuthorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace ConsiliumTempus.Infrastructure.UnitTests.Security.Authorization.Providers;

public class AuthorizationPolicyProviderTest
{
    #region Setup

    private readonly AuthorizationPolicyProvider _uut;

    public AuthorizationPolicyProviderTest()
    {
        var authOptions = Substitute.For<IOptions<AuthorizationOptions>>();
        var options = Substitute.For<AuthorizationOptions>();
        authOptions.Value.Returns(options);

        _uut = new AuthorizationPolicyProvider(authOptions);
    }

    #endregion

    [Fact]
    public async Task
        AuthorizationPolicyProvider_WhenItIsWorkspaceAuthorizationLevel_ShouldReturnNewPolicyWithWorkspaceAuthorizationRequirement()
    {
        // Arrange
        var policy = WorkspaceAuthorizationLevel.IsCollaborator.ToString();

        // Act
        var outcome = await _uut.GetPolicyAsync(policy);

        // Assert
        outcome!.Requirements.Should().HaveCount(1);
        outcome.Requirements[0].Should().BeOfType<WorkspaceAuthorizationRequirement>();
        ((WorkspaceAuthorizationRequirement)outcome.Requirements[0]).AuthorizationLevel.ToString().Should().Be(policy);
    }

    [Fact]
    public async Task 
        AuthorizationPolicyProvider_WhenItIsNotWorkspaceAuthorizationLevel_ShouldReturnNewPolicyWithPermissionRequirement()
    {
        // Arrange
        var policy = Permissions.DeleteProject.ToString();

        // Act
        var outcome = await _uut.GetPolicyAsync(policy);

        // Assert
        outcome!.Requirements.Should().HaveCount(1);
        outcome.Requirements[0].Should().BeOfType<PermissionRequirement>();
        ((PermissionRequirement)outcome.Requirements[0]).Permission.Should().Be(policy);
    }
}
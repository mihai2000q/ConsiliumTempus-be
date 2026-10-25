using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Infrastructure.Security.Authorization.Permission;
using ConsiliumTempus.Infrastructure.Security.Authorization.ProjectAuthorization;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using ConsiliumTempus.Infrastructure.Security.Authorization.WorkspaceAuthorization;
using ConsiliumTempus.Infrastructure.UnitTests.TestData.Security.Authorization;
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

    [Theory]
    [ClassData(typeof(AuthorizationPolicyProviderData.GetWorkspaceAuthorizationLevels))]
    public async Task
        AuthorizationPolicyProvider_WhenPolicyIsWorkspaceAuthorizationLevel_ShouldReturnNewPolicyWithWorkspaceAuthorizationRequirement(
            WorkspaceAuthorizationLevel workspaceAuthorizationLevel)
    {
        // Arrange
        var policy = workspaceAuthorizationLevel.ToString();

        // Act
        var outcome = await _uut.GetPolicyAsync(policy);

        // Assert
        outcome!.Requirements.Should().HaveCount(1);
        outcome.Requirements[0].Should().BeOfType<WorkspaceAuthorizationRequirement>();
        ((WorkspaceAuthorizationRequirement)outcome.Requirements[0]).AuthorizationLevel.ToString().Should().Be(policy);
    }

    [Theory]
    [ClassData(typeof(AuthorizationPolicyProviderData.GetProjectAuthorizationLevels))]
    public async Task
        AuthorizationPolicyProvider_WhenPolicyIsProjectAuthorizationLevel_ShouldReturnNewPolicyWithProjectAuthorizationRequirement(
            ProjectAuthorizationLevel projectAuthorizationLevel)
    {
        // Arrange
        var policy = projectAuthorizationLevel.ToString();

        // Act
        var outcome = await _uut.GetPolicyAsync(policy);

        // Assert
        outcome!.Requirements.Should().HaveCount(1);
        outcome.Requirements[0].Should().BeOfType<ProjectAuthorizationRequirement>();
        ((ProjectAuthorizationRequirement)outcome.Requirements[0]).AuthorizationLevel.ToString().Should().Be(policy);
    }

    [Theory]
    [ClassData(typeof(AuthorizationPolicyProviderData.GetPermissions))]
    public async Task
        AuthorizationPolicyProvider_WhenPolicyIsPermissions_ShouldReturnNewPolicyWithPermissionRequirement(
            Permissions permission)
    {
        // Arrange
        var policy = permission.ToString();

        // Act
        var outcome = await _uut.GetPolicyAsync(policy);

        // Assert
        outcome!.Requirements.Should().HaveCount(1);
        outcome.Requirements[0].Should().BeOfType<PermissionRequirement>();
        ((PermissionRequirement)outcome.Requirements[0]).Permission.ToString().Should().Be(policy);
    }

    [Theory]
    [InlineData("")]
    [InlineData("UnknownPolicy")]
    [InlineData("isallowed")]
    [InlineData("deleteproject")]
    public async Task AuthorizationPolicyProvider_WhenPolicyIsUnknown_ShouldReturnNull(
        string policy)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.GetPolicyAsync(policy);

        // Assert
        outcome.Should().BeNull();
    }
}
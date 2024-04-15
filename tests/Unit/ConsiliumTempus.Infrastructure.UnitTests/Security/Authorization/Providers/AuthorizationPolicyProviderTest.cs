using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Infrastructure.Security.Authorization.Permission;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace ConsiliumTempus.Infrastructure.UnitTests.Security.Authorization.Providers;

public class AuthorizationPolicyProviderTest
{
    #region Setup

    private readonly AuthorizationOptions _options;
    private readonly AuthorizationPolicyProvider _uut;

    public AuthorizationPolicyProviderTest()
    {
        var authOptions = Substitute.For<IOptions<AuthorizationOptions>>();
        _options = Substitute.For<AuthorizationOptions>();
        authOptions.Value.Returns(_options);
        
        _uut = new AuthorizationPolicyProvider(authOptions);
    }

    #endregion
    [Fact(Skip = "Cannot mock the Get Policy Call as it is an internal function")]
    public async Task WhenAuthPolicyProviderHasCachedPolicy_ShouldReturnCachedPolicy()
    {
        // Arrange
        var policyName = Permissions.DeleteProject.ToString();

        var policy = new AuthorizationPolicy([new PermissionRequirement(policyName)], []);
        _options
            .GetPolicy(policyName)
            .Returns(policy);

        // Act
        var outcome = await _uut.GetPolicyAsync(policyName);

        // Assert
        outcome.Should().Be(policy);
    }

    [Fact]
    public async Task WhenAuthPolicyProviderHasNotCached_ShouldReturnNewPolicy()
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
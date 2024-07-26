using System.Security.Claims;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Infrastructure.Security.Authorization.Permission;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using ConsiliumTempus.Infrastructure.UnitTests.TestData.Security.Authorization;
using ConsiliumTempus.Infrastructure.UnitTests.TestUtils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Infrastructure.UnitTests.Security.Authorization.Permission;

public class PermissionAuthorizationHandlerTest
{
    #region Setup

    private readonly IPermissionProvider _permissionProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWorkspaceProvider _workspaceProvider;
    private readonly PermissionAuthorizationHandler _uut;

    public PermissionAuthorizationHandlerTest()
    {
        _permissionProvider = Substitute.For<IPermissionProvider>();
        _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        _workspaceProvider = Substitute.For<IWorkspaceProvider>();

        var scope = Substitute.For<IServiceScope>();
        var scopeFactory = Substitute.For<IServiceScopeFactory>();
        scopeFactory
            .CreateScope()
            .Returns(scope);
        scope.ServiceProvider.GetService(typeof(IPermissionProvider)).Returns(_permissionProvider);
        scope.ServiceProvider.GetService(typeof(IHttpContextAccessor)).Returns(_httpContextAccessor);
        scope.ServiceProvider.GetService(typeof(IWorkspaceProvider)).Returns(_workspaceProvider);

        _uut = new PermissionAuthorizationHandler(scopeFactory);
    }

    #endregion

    [Fact]
    public async Task PermissionAuthorizationHandler_WhenSubClaimIsNull_ShouldFail()
    {
        // Arrange
        var requirements = new[] { new PermissionRequirement(Permissions.ReadWorkspace) };
        var user = new ClaimsPrincipal();
        var context = new AuthorizationHandlerContext(requirements, user, null);

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _httpContextAccessor.DidNotReceive();
        _workspaceProvider.DidNotReceive();
        _permissionProvider.DidNotReceive();

        context.HasSucceeded.Should().BeFalse();
    }

    [Fact]
    public async Task PermissionAuthorizationHandler_WhenSubClaimIsNotGuid_ShouldFail()
    {
        // Arrange
        var requirements = new[] { new PermissionRequirement(Permissions.ReadWorkspace) };
        var user = new ClaimsPrincipal();
        user.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, "")
        ]));
        var context = new AuthorizationHandlerContext(requirements, user, null);

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _httpContextAccessor.DidNotReceive();
        _workspaceProvider.DidNotReceive();
        _permissionProvider.DidNotReceive();

        context.HasSucceeded.Should().BeFalse();
    }

    [Fact]
    public async Task PermissionAuthorizationHandler_WhenRequestIsNull_ShouldFail()
    {
        // Arrange
        var requirements = new[] { new PermissionRequirement(Permissions.ReadWorkspace) };
        var user = new ClaimsPrincipal();
        user.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, user, null);

        _httpContextAccessor
            .HttpContext
            .ReturnsNull();

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(1)
            .HttpContext;
        _workspaceProvider.DidNotReceive();
        _permissionProvider.DidNotReceive();

        context.HasSucceeded.Should().BeFalse();
    }

    [Theory]
    [ClassData(typeof(PermissionAuthorizationHandlerData.GetPermissions))]
    public async Task PermissionAuthorizationHandler_WhenRequestIsEmpty_ShouldSucceed(
        Permissions permission,
        PermissionAuthorizationHandlerData.RequestLocation requestLocation,
        string? _1,
        PermissionAuthorizationHandlerData.StringIdType _2)
    {
        // Arrange
        var requirements = new[] { new PermissionRequirement(permission) };
        var user = new ClaimsPrincipal();
        user.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, user, null);

        Utils.Authorization.MockEmptyHttpRequest(_httpContextAccessor, requestLocation);

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(2)
            .HttpContext;
        _workspaceProvider.DidNotReceive();
        _permissionProvider.DidNotReceive();

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(PermissionAuthorizationHandlerData.GetPermissions))]
    public async Task PermissionAuthorizationHandler_WhenRequestIdIsEmpty_ShouldSucceed(
        Permissions permission,
        PermissionAuthorizationHandlerData.RequestLocation requestLocation,
        string? id,
        PermissionAuthorizationHandlerData.StringIdType _1)
    {
        // Arrange
        var requirements = new[] { new PermissionRequirement(permission) };
        var user = new ClaimsPrincipal();
        user.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, user, null);

        const string stringId = "";
        Utils.Authorization.MockHttpRequest(_httpContextAccessor, requestLocation, id, stringId);

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(2)
            .HttpContext;
        _workspaceProvider.DidNotReceive();
        _permissionProvider.DidNotReceive();

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(PermissionAuthorizationHandlerData.GetPermissions))]
    public async Task PermissionAuthorizationHandler_WhenRequestIdIsNotGuid_ShouldSucceed(
        Permissions permission,
        PermissionAuthorizationHandlerData.RequestLocation requestLocation,
        string? id,
        PermissionAuthorizationHandlerData.StringIdType _1)
    {
        // Arrange
        var requirements = new[] { new PermissionRequirement(permission) };
        var user = new ClaimsPrincipal();
        user.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, user, null);

        const string stringId = "not guid";
        Utils.Authorization.MockHttpRequest(_httpContextAccessor, requestLocation, id, stringId);

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(2)
            .HttpContext;
        _workspaceProvider.DidNotReceive();
        _permissionProvider.DidNotReceive();

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(PermissionAuthorizationHandlerData.GetPermissions))]
    public async Task PermissionAuthorizationHandler_WhenWorkspaceIsNull_ShouldSucceed(
        Permissions permission,
        PermissionAuthorizationHandlerData.RequestLocation requestLocation,
        string? id,
        PermissionAuthorizationHandlerData.StringIdType provider)
    {
        // Arrange
        var requirements = new[] { new PermissionRequirement(permission) };
        var user = new ClaimsPrincipal();
        user.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, user, null);

        var stringId = Guid.NewGuid().ToString();
        Utils.Authorization.MockHttpRequest(_httpContextAccessor, requestLocation, id, stringId);

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(2)
            .HttpContext;
        await Utils.Authorization.VerifyWorkspaceProvider(_workspaceProvider, provider, stringId);
        _permissionProvider.DidNotReceive();

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(PermissionAuthorizationHandlerData.GetPermissions))]
    public async Task PermissionAuthorizationHandler_WhenDoesNotHavePermission_ShouldFail(
        Permissions permission,
        PermissionAuthorizationHandlerData.RequestLocation requestLocation,
        string? id,
        PermissionAuthorizationHandlerData.StringIdType provider)
    {
        // Arrange
        var requirements = new[] { new PermissionRequirement(permission) };
        var userId = Guid.NewGuid().ToString();
        var user = new ClaimsPrincipal();
        user.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, userId)
        ]));
        var context = new AuthorizationHandlerContext(requirements, user, null);

        var stringId = Guid.NewGuid().ToString();
        Utils.Authorization.MockHttpRequest(_httpContextAccessor, requestLocation, id, stringId);

        var workspace = WorkspaceFactory.Create();
        Utils.Authorization.MockWorkspaceProvider(_workspaceProvider, workspace);

        _permissionProvider
            .GetPermissions(Arg.Any<UserId>(), Arg.Any<WorkspaceId>())
            .Returns([]);

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(2)
            .HttpContext;
        await Utils.Authorization.VerifyWorkspaceProvider(_workspaceProvider, provider, stringId);
        await _permissionProvider
            .Received(1)
            .GetPermissions(
                Arg.Is<UserId>(uId => uId.Value.ToString() == userId),
                Arg.Is<WorkspaceId>(wId => wId == workspace.Id));

        context.HasSucceeded.Should().BeFalse();
    }

    [Theory]
    [ClassData(typeof(PermissionAuthorizationHandlerData.GetPermissions))]
    public async Task PermissionAuthorizationHandler_WhenTheyHavePermission_ShouldSucceed(
        Permissions permission,
        PermissionAuthorizationHandlerData.RequestLocation requestLocation,
        string? id,
        PermissionAuthorizationHandlerData.StringIdType provider)
    {
        // Arrange
        var requirements = new[] { new PermissionRequirement(permission) };
        var userId = Guid.NewGuid().ToString();
        var user = new ClaimsPrincipal();
        user.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, userId)
        ]));
        var context = new AuthorizationHandlerContext(requirements, user, null);

        var stringId = Guid.NewGuid().ToString();
        Utils.Authorization.MockHttpRequest(_httpContextAccessor, requestLocation, id, stringId);

        var workspace = WorkspaceFactory.Create();
        Utils.Authorization.MockWorkspaceProvider(_workspaceProvider, workspace);

        _permissionProvider
            .GetPermissions(Arg.Any<UserId>(), Arg.Any<WorkspaceId>())
            .Returns([permission.ToString()]);

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(2)
            .HttpContext;
        await Utils.Authorization.VerifyWorkspaceProvider(_workspaceProvider, provider, stringId);
        await _permissionProvider
            .Received(1)
            .GetPermissions(
                Arg.Is<UserId>(uId => uId.Value.ToString() == userId),
                Arg.Is<WorkspaceId>(wId => wId == workspace.Id));

        context.HasSucceeded.Should().BeTrue();
    }
}
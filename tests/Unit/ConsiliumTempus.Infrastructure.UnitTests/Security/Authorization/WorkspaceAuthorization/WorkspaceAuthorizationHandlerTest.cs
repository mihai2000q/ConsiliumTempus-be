using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ConsiliumTempus.Common.UnitTests.Common.Entities;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using ConsiliumTempus.Infrastructure.Security.Authorization.WorkspaceAuthorization;
using ConsiliumTempus.Infrastructure.UnitTests.TestData.Security.Authorization;
using ConsiliumTempus.Infrastructure.UnitTests.TestUtils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Infrastructure.UnitTests.Security.Authorization.WorkspaceAuthorization;

public class WorkspaceAuthorizationHandlerTest
{
    #region Setup

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWorkspaceProvider _workspaceProvider;
    private readonly WorkspaceAuthorizationHandler _uut;

    public WorkspaceAuthorizationHandlerTest()
    {
        _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        _workspaceProvider = Substitute.For<IWorkspaceProvider>();

        var scope = Substitute.For<IServiceScope>();
        var scopeFactory = Substitute.For<IServiceScopeFactory>();
        scopeFactory
            .CreateScope()
            .Returns(scope);
        scope.ServiceProvider.GetService(typeof(IHttpContextAccessor)).Returns(_httpContextAccessor);
        scope.ServiceProvider.GetService(typeof(IWorkspaceProvider)).Returns(_workspaceProvider);

        _uut = new WorkspaceAuthorizationHandler(scopeFactory);
    }

    #endregion

    [Fact]
    public async Task WorkspaceAuthorizationHandler_WhenSubClaimIsNull_ShouldFail()
    {
        // Arrange
        var requirements = new[] { new WorkspaceAuthorizationRequirement(WorkspaceAuthorizationLevel.IsCollaborator) };
        var userClaims = new ClaimsPrincipal();
        var context = new AuthorizationHandlerContext(requirements, userClaims, null);

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _httpContextAccessor.DidNotReceive();
        _workspaceProvider.DidNotReceive();

        context.HasSucceeded.Should().BeFalse();
    }

    [Fact]
    public async Task WorkspaceAuthorizationHandler_WhenSubClaimIsNotGuid_ShouldFail()
    {
        // Arrange
        var requirements = new[] { new WorkspaceAuthorizationRequirement(WorkspaceAuthorizationLevel.IsCollaborator) };
        var userClaims = new ClaimsPrincipal();
        userClaims.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, "")
        ]));
        var context = new AuthorizationHandlerContext(requirements, userClaims, null);

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _httpContextAccessor.DidNotReceive();
        _workspaceProvider.DidNotReceive();

        context.HasSucceeded.Should().BeFalse();
    }

    [Fact]
    public async Task WorkspaceAuthorizationHandler_WhenRequestIsNull_ShouldFail()
    {
        // Arrange
        var requirements = new[] { new WorkspaceAuthorizationRequirement(WorkspaceAuthorizationLevel.IsCollaborator) };
        var userClaims = new ClaimsPrincipal();
        userClaims.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, userClaims, null);

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

        context.HasSucceeded.Should().BeFalse();
    }

    [Theory]
    [ClassData(typeof(WorkspaceAuthorizationHandlerData.GetAuthorizationLevels))]
    public async Task ProjectAuthorizationHandler_WhenRequestControllerIsEmpty_ShouldSucceed(
        WorkspaceAuthorizationLevel workspaceAuthorizationLevel,
        WorkspaceAuthorizationHandlerData.Controller _1,
        WorkspaceAuthorizationHandlerData.Method _2,
        WorkspaceAuthorizationHandlerData.RequestLocation _3,
        WorkspaceAuthorizationHandlerData.StringIdType _4)
    {
        // Arrange
        var requirements = new[] { new WorkspaceAuthorizationRequirement(workspaceAuthorizationLevel) };
        var userClaims = new ClaimsPrincipal();
        userClaims.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, userClaims, null);

        _httpContextAccessor
            .HttpContext!
            .Request
            .RouteValues
            .Returns(new RouteValueDictionary());

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(2)
            .HttpContext;
        _workspaceProvider.DidNotReceive();

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(WorkspaceAuthorizationHandlerData.GetAuthorizationLevels))]
    public async Task ProjectAuthorizationHandler_WhenRequestMethodIsEmpty_ShouldSucceed(
        WorkspaceAuthorizationLevel workspaceAuthorizationLevel,
        WorkspaceAuthorizationHandlerData.Controller controller,
        WorkspaceAuthorizationHandlerData.Method _1,
        WorkspaceAuthorizationHandlerData.RequestLocation _2,
        WorkspaceAuthorizationHandlerData.StringIdType _3)
    {
        // Arrange
        var requirements = new[] { new WorkspaceAuthorizationRequirement(workspaceAuthorizationLevel) };
        var userClaims = new ClaimsPrincipal();
        userClaims.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, userClaims, null);

        _httpContextAccessor
            .HttpContext!
            .Request
            .RouteValues
            .Returns(new RouteValueDictionary { ["controller"] = controller.ToString() });

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(2)
            .HttpContext;
        _workspaceProvider.DidNotReceive();

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(WorkspaceAuthorizationHandlerData.GetAuthorizationLevels))]
    public async Task WorkspaceAuthorizationHandler_WhenRequestIsEmpty_ShouldSucceed(
        WorkspaceAuthorizationLevel workspaceAuthorizationLevel,
        WorkspaceAuthorizationHandlerData.Controller controller,
        WorkspaceAuthorizationHandlerData.Method method,
        WorkspaceAuthorizationHandlerData.RequestLocation requestLocation,
        WorkspaceAuthorizationHandlerData.StringIdType _1)
    {
        // Arrange
        var requirements = new[] { new WorkspaceAuthorizationRequirement(workspaceAuthorizationLevel) };
        var userClaims = new ClaimsPrincipal();
        userClaims.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, userClaims, null);

        Utils.Authorization.Workspace.MockEmptyHttpRequest(_httpContextAccessor, controller, method, requestLocation);

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(requestLocation == WorkspaceAuthorizationHandlerData.RequestLocation.Route ? 2 : 3)
            .HttpContext;
        _workspaceProvider.DidNotReceive();

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(WorkspaceAuthorizationHandlerData.GetAuthorizationLevels))]
    public async Task WorkspaceAuthorizationHandler_WhenRequestIdIsEmpty_ShouldSucceed(
        WorkspaceAuthorizationLevel workspaceAuthorizationLevel,
        WorkspaceAuthorizationHandlerData.Controller controller,
        WorkspaceAuthorizationHandlerData.Method method,
        WorkspaceAuthorizationHandlerData.RequestLocation requestLocation,
        WorkspaceAuthorizationHandlerData.StringIdType _1)
    {
        // Arrange
        var requirements = new[] { new WorkspaceAuthorizationRequirement(workspaceAuthorizationLevel) };
        var userClaims = new ClaimsPrincipal();
        userClaims.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, userClaims, null);

        const string stringId = "";
        Utils.Authorization.Workspace.MockHttpRequest(
            _httpContextAccessor,
            controller,
            method,
            requestLocation,
            stringId);

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(requestLocation == WorkspaceAuthorizationHandlerData.RequestLocation.Route ? 2 : 3)
            .HttpContext;
        _workspaceProvider.DidNotReceive();

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(WorkspaceAuthorizationHandlerData.GetAuthorizationLevels))]
    public async Task WorkspaceAuthorizationHandler_WhenRequestIdIsNotGuid_ShouldSucceed(
        WorkspaceAuthorizationLevel workspaceAuthorizationLevel,
        WorkspaceAuthorizationHandlerData.Controller controller,
        WorkspaceAuthorizationHandlerData.Method method,
        WorkspaceAuthorizationHandlerData.RequestLocation requestLocation,
        WorkspaceAuthorizationHandlerData.StringIdType _1)
    {
        // Arrange
        var requirements = new[] { new WorkspaceAuthorizationRequirement(workspaceAuthorizationLevel) };
        var userClaims = new ClaimsPrincipal();
        userClaims.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, userClaims, null);

        const string stringId = "not guid";
        Utils.Authorization.Workspace.MockHttpRequest(
            _httpContextAccessor,
            controller,
            method,
            requestLocation,
            stringId);

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(requestLocation == WorkspaceAuthorizationHandlerData.RequestLocation.Route ? 2 : 3)
            .HttpContext;
        _workspaceProvider.DidNotReceive();

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(WorkspaceAuthorizationHandlerData.GetAuthorizationLevels))]
    public async Task WorkspaceAuthorizationHandler_WhenWorkspaceIsNull_ShouldSucceed(
        WorkspaceAuthorizationLevel workspaceAuthorizationLevel,
        WorkspaceAuthorizationHandlerData.Controller controller,
        WorkspaceAuthorizationHandlerData.Method method,
        WorkspaceAuthorizationHandlerData.RequestLocation requestLocation,
        WorkspaceAuthorizationHandlerData.StringIdType stringIdType)
    {
        // Arrange
        var requirements = new[] { new WorkspaceAuthorizationRequirement(workspaceAuthorizationLevel) };
        var user = new ClaimsPrincipal();
        user.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, user, null);

        var stringId = Guid.NewGuid().ToString();
        Utils.Authorization.Workspace.MockHttpRequest(
            _httpContextAccessor,
            controller,
            method,
            requestLocation,
            stringId);

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(requestLocation == WorkspaceAuthorizationHandlerData.RequestLocation.Route ? 2 : 3)
            .HttpContext;
        await Utils.Authorization.Workspace.VerifyWorkspaceProvider(_workspaceProvider, stringIdType, stringId);

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(WorkspaceAuthorizationHandlerData.GetAuthorizationLevels))]
    public async Task WorkspaceAuthorizationHandler_WhenNotAuthorized_ShouldFail(
        WorkspaceAuthorizationLevel workspaceAuthorizationLevel,
        WorkspaceAuthorizationHandlerData.Controller controller,
        WorkspaceAuthorizationHandlerData.Method method,
        WorkspaceAuthorizationHandlerData.RequestLocation requestLocation,
        WorkspaceAuthorizationHandlerData.StringIdType stringIdType)
    {
        // Arrange
        var requirements = new[] { new WorkspaceAuthorizationRequirement(workspaceAuthorizationLevel) };
        var userClaims = new ClaimsPrincipal();
        userClaims.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, userClaims, null);

        var workspace = WorkspaceFactory.Create();
        Utils.Authorization.Workspace.MockWorkspaceProvider(_workspaceProvider, workspace);

        var stringId = workspace.Id.ToString();
        Utils.Authorization.Workspace.MockHttpRequest(
            _httpContextAccessor,
            controller,
            method,
            requestLocation,
            stringId);

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(requestLocation == WorkspaceAuthorizationHandlerData.RequestLocation.Route ? 2 : 3)
            .HttpContext;
        await Utils.Authorization.Workspace.VerifyWorkspaceProvider(_workspaceProvider, stringIdType, stringId);

        context.HasSucceeded.Should().BeFalse();
    }

    [Theory]
    [ClassData(typeof(WorkspaceAuthorizationHandlerData.GetAuthorizationLevels))]
    public async Task WorkspaceAuthorizationHandler_WhenIsAuthorized_ShouldSucceed(
        WorkspaceAuthorizationLevel workspaceAuthorizationLevel,
        WorkspaceAuthorizationHandlerData.Controller controller,
        WorkspaceAuthorizationHandlerData.Method method,
        WorkspaceAuthorizationHandlerData.RequestLocation requestLocation,
        WorkspaceAuthorizationHandlerData.StringIdType stringIdType)
    {
        // Arrange
        var requirements = new[] { new WorkspaceAuthorizationRequirement(workspaceAuthorizationLevel) };
        var user = UserFactory.Create();
        var userClaims = new ClaimsPrincipal();
        userClaims.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, userClaims, null);

        var workspace = WorkspaceFactory.Create(owner: user);
        workspace.AddUserMembership(MembershipFactory.Create(user));
        Utils.Authorization.Workspace.MockWorkspaceProvider(_workspaceProvider, workspace);

        var stringId = workspace.Id.ToString();
        Utils.Authorization.Workspace.MockHttpRequest(
            _httpContextAccessor,
            controller,
            method,
            requestLocation,
            stringId);

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(requestLocation == WorkspaceAuthorizationHandlerData.RequestLocation.Route ? 2 : 3)
            .HttpContext;
        await Utils.Authorization.Workspace.VerifyWorkspaceProvider(_workspaceProvider, stringIdType, stringId);

        context.HasSucceeded.Should().BeTrue();
    }
}
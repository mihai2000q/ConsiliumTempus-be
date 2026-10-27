using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Infrastructure.Security.Authorization.NotFound;
using ErrorOr;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ConsiliumTempus.Common.UnitTests.Common.Entities;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
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

    private async Task VerifyWorkspaceProviderDidNotReceiveAny()
    {
        await _workspaceProvider.DidNotReceive().GetWithMemberships(Arg.Any<WorkspaceId>());
        await _workspaceProvider.DidNotReceive().GetByProjectWithMemberships(Arg.Any<ProjectId>());
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
        _ = _httpContextAccessor.DidNotReceive().HttpContext;
        await VerifyWorkspaceProviderDidNotReceiveAny();

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
        _ = _httpContextAccessor.DidNotReceive().HttpContext;
        await VerifyWorkspaceProviderDidNotReceiveAny();

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
        await VerifyWorkspaceProviderDidNotReceiveAny();

        context.HasSucceeded.Should().BeFalse();
    }

    [Theory]
    [ClassData(typeof(WorkspaceAuthorizationHandlerData.GetAuthorizationLevels))]
    public async Task WorkspaceAuthorizationHandler_WhenRequestControllerIsEmpty_ShouldSucceed(
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
        await VerifyWorkspaceProviderDidNotReceiveAny();

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(WorkspaceAuthorizationHandlerData.GetAuthorizationLevels))]
    public async Task WorkspaceAuthorizationHandler_WhenRequestMethodIsEmpty_ShouldSucceed(
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
        await VerifyWorkspaceProviderDidNotReceiveAny();

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
            .Received(requestLocation == WorkspaceAuthorizationHandlerData.RequestLocation.Route ? 3 : 4)
            .HttpContext;
        await VerifyWorkspaceProviderDidNotReceiveAny();

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
            .Received(requestLocation == WorkspaceAuthorizationHandlerData.RequestLocation.Route ? 3 : 4)
            .HttpContext;
        await VerifyWorkspaceProviderDidNotReceiveAny();

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
            .Received(requestLocation == WorkspaceAuthorizationHandlerData.RequestLocation.Route ? 3 : 4)
            .HttpContext;
        await VerifyWorkspaceProviderDidNotReceiveAny();

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(WorkspaceAuthorizationHandlerData.GetAuthorizationLevels))]
    public async Task WorkspaceAuthorizationHandler_WhenRequestIdIsEmptyGuid_ShouldSucceed(
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

        var stringId = Guid.Empty.ToString();
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
            .Received(requestLocation == WorkspaceAuthorizationHandlerData.RequestLocation.Route ? 3 : 4)
            .HttpContext;
        await VerifyWorkspaceProviderDidNotReceiveAny();

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(WorkspaceAuthorizationHandlerData.GetAuthorizationLevels))]
    public async Task WorkspaceAuthorizationHandler_WhenWorkspaceIsNull_ShouldFailAsNotFound(
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
            .Received(requestLocation == WorkspaceAuthorizationHandlerData.RequestLocation.Route ? 3 : 4)
            .HttpContext;
        await Utils.Authorization.Workspace.VerifyWorkspaceProvider(_workspaceProvider, stringIdType, stringId);

        context.HasSucceeded.Should().BeFalse();
        context.HasFailed.Should().BeTrue();
        context.FailureReasons.OfType<NotFoundAuthorizationFailureReason>().Should().ContainSingle()
            .Which.Error.Should().Be(GetNotFoundError(stringIdType));
    }

    [Fact]
    public async Task
        WorkspaceAuthorizationHandler_WhenAuthorizationLevelIsNotSupported_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var requirements = new[] { new WorkspaceAuthorizationRequirement((WorkspaceAuthorizationLevel)int.MaxValue) };
        var userClaims = new ClaimsPrincipal();
        userClaims.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, userClaims, null);

        var workspace = WorkspaceFactory.Create();
        Utils.Authorization.Workspace.MockWorkspaceProvider(_workspaceProvider, workspace);

        Utils.Authorization.Workspace.MockHttpRequest(
            _httpContextAccessor,
            WorkspaceAuthorizationHandlerData.Controller.Workspace,
            WorkspaceAuthorizationHandlerData.Method.DELETE,
            WorkspaceAuthorizationHandlerData.RequestLocation.Route,
            workspace.Id.ToString());

        // Act
        var outcome = async () => await _uut.HandleAsync(context);

        // Assert
        await outcome.Should().ThrowAsync<ArgumentOutOfRangeException>();

        context.HasSucceeded.Should().BeFalse();
    }

    [Theory]
    [ClassData(typeof(WorkspaceAuthorizationHandlerData.GetAuthorizationLevels))]
    public async Task WorkspaceAuthorizationHandler_WhenIsNotCollaborator_ShouldFailAsNotFound(
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
            .Received(requestLocation == WorkspaceAuthorizationHandlerData.RequestLocation.Route ? 3 : 4)
            .HttpContext;
        await Utils.Authorization.Workspace.VerifyWorkspaceProvider(_workspaceProvider, stringIdType, stringId);

        context.HasSucceeded.Should().BeFalse();
        context.HasFailed.Should().BeTrue();
        context.FailureReasons.OfType<NotFoundAuthorizationFailureReason>().Should().ContainSingle()
            .Which.Error.Should().Be(GetNotFoundError(stringIdType));
    }

    [Theory]
    [ClassData(typeof(WorkspaceAuthorizationHandlerData.GetAuthorizationLevels))]
    public async Task WorkspaceAuthorizationHandler_WhenIsCollaboratorButNotOwner_ShouldSucceedOnlyForCollaboratorLevel(
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

        var workspace = WorkspaceFactory.Create();
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
            .Received(requestLocation == WorkspaceAuthorizationHandlerData.RequestLocation.Route ? 3 : 4)
            .HttpContext;
        await Utils.Authorization.Workspace.VerifyWorkspaceProvider(_workspaceProvider, stringIdType, stringId);

        context.HasSucceeded.Should().Be(workspaceAuthorizationLevel == WorkspaceAuthorizationLevel.IsCollaborator);
        context.FailureReasons.Should().BeEmpty(); // the user knows that the workspace exists
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
            .Received(requestLocation == WorkspaceAuthorizationHandlerData.RequestLocation.Route ? 3 : 4)
            .HttpContext;
        await Utils.Authorization.Workspace.VerifyWorkspaceProvider(_workspaceProvider, stringIdType, stringId);

        context.HasSucceeded.Should().BeTrue();
    }

    private static Error GetNotFoundError(WorkspaceAuthorizationHandlerData.StringIdType stringIdType)
    {
        return stringIdType switch
        {
            WorkspaceAuthorizationHandlerData.StringIdType.Workspace => Errors.Workspace.NotFound,
            WorkspaceAuthorizationHandlerData.StringIdType.Project => Errors.Project.NotFound,
            _ => throw new ArgumentOutOfRangeException(nameof(stringIdType))
        };
    }
}

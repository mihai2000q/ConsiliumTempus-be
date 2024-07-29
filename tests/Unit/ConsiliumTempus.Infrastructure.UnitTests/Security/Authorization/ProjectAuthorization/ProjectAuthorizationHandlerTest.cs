using System.Security.Claims;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Infrastructure.Security.Authorization.ProjectAuthorization;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using ConsiliumTempus.Infrastructure.UnitTests.TestData.Security.Authorization;
using ConsiliumTempus.Infrastructure.UnitTests.TestUtils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Infrastructure.UnitTests.Security.Authorization.ProjectAuthorization;

public class ProjectAuthorizationHandlerTest
{
    #region Setup

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProjectProvider _projectProvider;
    private readonly ProjectAuthorizationHandler _uut;

    public ProjectAuthorizationHandlerTest()
    {
        _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        _projectProvider = Substitute.For<IProjectProvider>();

        var scope = Substitute.For<IServiceScope>();
        var scopeFactory = Substitute.For<IServiceScopeFactory>();
        scopeFactory
            .CreateScope()
            .Returns(scope);
        scope.ServiceProvider.GetService(typeof(IHttpContextAccessor)).Returns(_httpContextAccessor);
        scope.ServiceProvider.GetService(typeof(IProjectProvider)).Returns(_projectProvider);

        _uut = new ProjectAuthorizationHandler(scopeFactory);
    }

    #endregion

    [Fact]
    public async Task ProjectAuthorizationHandler_WhenSubClaimIsNull_ShouldFail()
    {
        // Arrange
        var requirements = new[] { new ProjectAuthorizationRequirement(ProjectAuthorizationLevel.IsAllowed) };
        var user = new ClaimsPrincipal();
        var context = new AuthorizationHandlerContext(requirements, user, null);

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _httpContextAccessor.DidNotReceive();
        _projectProvider.DidNotReceive();

        context.HasSucceeded.Should().BeFalse();
    }

    [Fact]
    public async Task ProjectAuthorizationHandler_WhenSubClaimIsNotGuid_ShouldFail()
    {
        // Arrange
        var requirements = new[] { new ProjectAuthorizationRequirement(ProjectAuthorizationLevel.IsAllowed) };
        var user = new ClaimsPrincipal();
        user.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, "")
        ]));
        var context = new AuthorizationHandlerContext(requirements, user, null);

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _httpContextAccessor.DidNotReceive();
        _projectProvider.DidNotReceive();

        context.HasSucceeded.Should().BeFalse();
    }

    [Fact]
    public async Task ProjectAuthorizationHandler_WhenRequestIsNull_ShouldFail()
    {
        // Arrange
        var requirements = new[] { new ProjectAuthorizationRequirement(ProjectAuthorizationLevel.IsAllowed) };
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
        _projectProvider.DidNotReceive();

        context.HasSucceeded.Should().BeFalse();
    }

    [Theory]
    [ClassData(typeof(ProjectAuthorizationHandlerData.GetAuthorizationLevels))]
    public async Task ProjectAuthorizationHandler_WhenRequestControllerIsEmpty_ShouldSucceed(
        ProjectAuthorizationLevel projectAuthorizationLevel,
        ProjectAuthorizationHandlerData.RequestLocation _1,
        Type? _2,
        ProjectAuthorizationHandlerData.Controller _3,
        string _4,
        ProjectAuthorizationHandlerData.StringIdType _5)
    {
        // Arrange
        var requirements = new[] { new ProjectAuthorizationRequirement(projectAuthorizationLevel) };
        var user = new ClaimsPrincipal();
        user.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, user, null);

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
        _projectProvider.DidNotReceive();

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(ProjectAuthorizationHandlerData.GetAuthorizationLevels))]
    public async Task ProjectAuthorizationHandler_WhenRequestActionIsEmpty_ShouldSucceed(
        ProjectAuthorizationLevel projectAuthorizationLevel,
        ProjectAuthorizationHandlerData.RequestLocation _1,
        Type? _2,
        ProjectAuthorizationHandlerData.Controller controller,
        string _3,
        ProjectAuthorizationHandlerData.StringIdType _4)
    {
        // Arrange
        var requirements = new[] { new ProjectAuthorizationRequirement(projectAuthorizationLevel) };
        var user = new ClaimsPrincipal();
        user.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, user, null);

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
        _projectProvider.DidNotReceive();

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(ProjectAuthorizationHandlerData.GetAuthorizationLevels))]
    public async Task ProjectAuthorizationHandler_WhenRequestIsEmpty_ShouldSucceed(
        ProjectAuthorizationLevel projectAuthorizationLevel,
        ProjectAuthorizationHandlerData.RequestLocation requestLocation,
        Type? _1,
        ProjectAuthorizationHandlerData.Controller controller,
        string requestAction,
        ProjectAuthorizationHandlerData.StringIdType _2)
    {
        // Arrange
        var requirements = new[] { new ProjectAuthorizationRequirement(projectAuthorizationLevel) };
        var user = new ClaimsPrincipal();
        user.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, user, null);

        Utils.Authorization.MockEmptyHttpRequest(_httpContextAccessor, requestLocation, controller, requestAction);

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(requestLocation == ProjectAuthorizationHandlerData.RequestLocation.Route ? 2 : 3)
            .HttpContext;
        _projectProvider.DidNotReceive();

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(ProjectAuthorizationHandlerData.GetAuthorizationLevels))]
    public async Task ProjectAuthorizationHandler_WhenRequestIdIsEmpty_ShouldSucceed(
        ProjectAuthorizationLevel projectAuthorizationLevel,
        ProjectAuthorizationHandlerData.RequestLocation requestLocation,
        Type? idType,
        ProjectAuthorizationHandlerData.Controller controller,
        string requestAction,
        ProjectAuthorizationHandlerData.StringIdType _1)
    {
        // Arrange
        var requirements = new[] { new ProjectAuthorizationRequirement(projectAuthorizationLevel) };
        var user = new ClaimsPrincipal();
        user.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, user, null);

        const string stringId = "";
        Utils.Authorization.MockHttpRequest(
            _httpContextAccessor,
            requestLocation,
            idType,
            controller,
            requestAction,
            stringId);

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(requestLocation == ProjectAuthorizationHandlerData.RequestLocation.Route ? 2 : 3)
            .HttpContext;
        _projectProvider.DidNotReceive();

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(ProjectAuthorizationHandlerData.GetAuthorizationLevels))]
    public async Task ProjectAuthorizationHandler_WhenRequestIdIsNotGuid_ShouldSucceed(
        ProjectAuthorizationLevel projectAuthorizationLevel,
        ProjectAuthorizationHandlerData.RequestLocation requestLocation,
        Type? idType,
        ProjectAuthorizationHandlerData.Controller controller,
        string requestAction,
        ProjectAuthorizationHandlerData.StringIdType _1)
    {
        // Arrange
        var requirements = new[] { new ProjectAuthorizationRequirement(projectAuthorizationLevel) };
        var user = new ClaimsPrincipal();
        user.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, user, null);

        const string stringId = "not guid";
        Utils.Authorization.MockHttpRequest(
            _httpContextAccessor,
            requestLocation,
            idType,
            controller,
            requestAction,
            stringId);

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(requestLocation == ProjectAuthorizationHandlerData.RequestLocation.Route ? 2 : 3)
            .HttpContext;
        _projectProvider.DidNotReceive();

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(ProjectAuthorizationHandlerData.GetAuthorizationLevels))]
    public async Task ProjectAuthorizationHandler_WhenProjectIsNull_ShouldSucceed(
        ProjectAuthorizationLevel projectAuthorizationLevel,
        ProjectAuthorizationHandlerData.RequestLocation requestLocation,
        Type? idType,
        ProjectAuthorizationHandlerData.Controller controller,
        string requestAction,
        ProjectAuthorizationHandlerData.StringIdType stringIdType)
    {
        // Arrange
        var requirements = new[] { new ProjectAuthorizationRequirement(projectAuthorizationLevel) };
        var user = UserFactory.Create();
        var userClaims = new ClaimsPrincipal();
        userClaims.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, userClaims, null);

        var stringId = Guid.NewGuid().ToString();
        Utils.Authorization.MockHttpRequest(
            _httpContextAccessor,
            requestLocation,
            idType,
            controller,
            requestAction,
            stringId);

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(requestLocation == ProjectAuthorizationHandlerData.RequestLocation.Route ? 2 : 3)
            .HttpContext;
        await Utils.Authorization.VerifyProjectProvider(_projectProvider, stringIdType, stringId);

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(ProjectAuthorizationHandlerData.GetAuthorizationLevels))]
    public async Task ProjectAuthorizationHandler_WhenNotAuthorized_ShouldFail(
        ProjectAuthorizationLevel projectAuthorizationLevel,
        ProjectAuthorizationHandlerData.RequestLocation requestLocation,
        Type? idType,
        ProjectAuthorizationHandlerData.Controller controller,
        string requestAction,
        ProjectAuthorizationHandlerData.StringIdType stringIdType)
    {
        // Arrange
        var requirements = new[] { new ProjectAuthorizationRequirement(projectAuthorizationLevel) };
        var user = UserFactory.Create();
        var userClaims = new ClaimsPrincipal();
        userClaims.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, userClaims, null);

        var project = ProjectFactory.Create();
        Utils.Authorization.MockProjectProvider(_projectProvider, project);

        var stringId = project.Id.Value.ToString();
        Utils.Authorization.MockHttpRequest(
            _httpContextAccessor,
            requestLocation,
            idType,
            controller,
            requestAction,
            stringId);

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(requestLocation == ProjectAuthorizationHandlerData.RequestLocation.Route ? 2 : 3)
            .HttpContext;
        await Utils.Authorization.VerifyProjectProvider(_projectProvider, stringIdType, stringId);

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(ProjectAuthorizationHandlerData.GetAuthorizationLevels))]
    public async Task ProjectAuthorizationHandler_WhenIsAuthorized_ShouldSucceed(
        ProjectAuthorizationLevel projectAuthorizationLevel,
        ProjectAuthorizationHandlerData.RequestLocation requestLocation,
        Type? idType,
        ProjectAuthorizationHandlerData.Controller controller,
        string requestAction,
        ProjectAuthorizationHandlerData.StringIdType stringIdType)
    {
        // Arrange
        var requirements = new[] { new ProjectAuthorizationRequirement(projectAuthorizationLevel) };
        var user = UserFactory.Create();
        var userClaims = new ClaimsPrincipal();
        userClaims.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, userClaims, null);

        var project = ProjectFactory.Create(owner: user);
        project.AddAllowedMember(user);
        Utils.Authorization.MockProjectProvider(_projectProvider, project);

        var stringId = project.Id.Value.ToString();
        Utils.Authorization.MockHttpRequest(
            _httpContextAccessor,
            requestLocation,
            idType,
            controller,
            requestAction,
            stringId);

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(requestLocation == ProjectAuthorizationHandlerData.RequestLocation.Route ? 2 : 3)
            .HttpContext;
        await Utils.Authorization.VerifyProjectProvider(_projectProvider, stringIdType, stringId);

        context.HasSucceeded.Should().BeTrue();
    }
}
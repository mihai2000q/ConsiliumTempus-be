using ConsiliumTempus.Infrastructure.Security.Authorization.NotFound;
using System.Security.Claims;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
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
using ConsiliumTempus.Infrastructure.Security.Authorization.Targets;
using Action = ConsiliumTempus.Infrastructure.UnitTests.TestUtils.Utils.Authorization.Action;
using Controller = ConsiliumTempus.Infrastructure.UnitTests.TestUtils.Utils.Authorization.Controller;

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

    private async Task VerifyProjectProviderDidNotReceiveAny()
    {
        await _projectProvider.DidNotReceive().Get(Arg.Any<ProjectId>());
        await _projectProvider.DidNotReceive().GetByCustomFieldSetup(Arg.Any<CustomFieldSetupId>());
        await _projectProvider.DidNotReceive().GetByProjectSprint(Arg.Any<ProjectSprintId>());
        await _projectProvider.DidNotReceive().GetByProjectStage(Arg.Any<ProjectStageId>());
        await _projectProvider.DidNotReceive().GetByProjectTask(Arg.Any<ProjectTaskId>());
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
        _ = _httpContextAccessor.DidNotReceive().HttpContext;
        await VerifyProjectProviderDidNotReceiveAny();

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
        _ = _httpContextAccessor.DidNotReceive().HttpContext;
        await VerifyProjectProviderDidNotReceiveAny();

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
        await VerifyProjectProviderDidNotReceiveAny();

        context.HasSucceeded.Should().BeFalse();
    }

    [Theory]
    [ClassData(typeof(ProjectAuthorizationHandlerData.GetIsAllowed))]
    [ClassData(typeof(ProjectAuthorizationHandlerData.GetIsOwner))]
    internal async Task ProjectAuthorizationHandler_WhenRequestControllerIsEmpty_ShouldThrowForUnmappedEndpoint(
        ProjectAuthorizationLevel projectAuthorizationLevel,
        IdLocation _1,
        Type? _2,
        Controller _3,
        Action _4,
        TargetKind _5)
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
        var act = async () => await _uut.HandleAsync(context);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        await VerifyProjectProviderDidNotReceiveAny();

        context.HasSucceeded.Should().BeFalse();
    }

    [Theory]
    [ClassData(typeof(ProjectAuthorizationHandlerData.GetIsAllowed))]
    [ClassData(typeof(ProjectAuthorizationHandlerData.GetIsOwner))]
    internal async Task ProjectAuthorizationHandler_WhenRequestMethodIsEmpty_ShouldThrowForUnmappedEndpoint(
        ProjectAuthorizationLevel projectAuthorizationLevel,
        IdLocation _1,
        Type? _2,
        Controller controller,
        Action _3,
        TargetKind _4)
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
        var act = async () => await _uut.HandleAsync(context);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        await VerifyProjectProviderDidNotReceiveAny();

        context.HasSucceeded.Should().BeFalse();
    }

    [Theory]
    [ClassData(typeof(ProjectAuthorizationHandlerData.GetIsAllowed))]
    [ClassData(typeof(ProjectAuthorizationHandlerData.GetIsOwner))]
    internal async Task ProjectAuthorizationHandler_WhenRequestIsEmpty_ShouldSucceed(
        ProjectAuthorizationLevel projectAuthorizationLevel,
        IdLocation requestLocation,
        Type? _1,
        Controller controller,
        Action requestAction,
        TargetKind _2)
    {
        // Arrange
        var requirements = new[] { new ProjectAuthorizationRequirement(projectAuthorizationLevel) };
        var user = new ClaimsPrincipal();
        user.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, user, null);

        Utils.Authorization.Project.MockEmptyHttpRequest(_httpContextAccessor, requestLocation, controller,
            requestAction);

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(2)
            .HttpContext;
        await VerifyProjectProviderDidNotReceiveAny();

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(ProjectAuthorizationHandlerData.GetIsAllowed))]
    [ClassData(typeof(ProjectAuthorizationHandlerData.GetIsOwner))]
    internal async Task ProjectAuthorizationHandler_WhenRequestIdIsEmpty_ShouldSucceed(
        ProjectAuthorizationLevel projectAuthorizationLevel,
        IdLocation requestLocation,
        Type? idType,
        Controller controller,
        Action requestAction,
        TargetKind _1)
    {
        // Arrange
        var requirements = new[] { new ProjectAuthorizationRequirement(projectAuthorizationLevel) };
        var user = new ClaimsPrincipal();
        user.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, user, null);

        const string stringId = "";
        Utils.Authorization.Project.MockHttpRequest(
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
            .Received(2)
            .HttpContext;
        await VerifyProjectProviderDidNotReceiveAny();

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(ProjectAuthorizationHandlerData.GetIsAllowed))]
    [ClassData(typeof(ProjectAuthorizationHandlerData.GetIsOwner))]
    internal async Task ProjectAuthorizationHandler_WhenRequestIdIsNotGuid_ShouldSucceed(
        ProjectAuthorizationLevel projectAuthorizationLevel,
        IdLocation requestLocation,
        Type? idType,
        Controller controller,
        Action requestAction,
        TargetKind _1)
    {
        // Arrange
        var requirements = new[] { new ProjectAuthorizationRequirement(projectAuthorizationLevel) };
        var user = new ClaimsPrincipal();
        user.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, user, null);

        const string stringId = "not guid";
        Utils.Authorization.Project.MockHttpRequest(
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
            .Received(2)
            .HttpContext;
        await VerifyProjectProviderDidNotReceiveAny();

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(ProjectAuthorizationHandlerData.GetIsAllowed))]
    [ClassData(typeof(ProjectAuthorizationHandlerData.GetIsOwner))]
    internal async Task ProjectAuthorizationHandler_WhenRequestIdIsEmptyGuid_ShouldSucceed(
        ProjectAuthorizationLevel projectAuthorizationLevel,
        IdLocation requestLocation,
        Type? idType,
        Controller controller,
        Action requestAction,
        TargetKind _1)
    {
        // Arrange
        var requirements = new[] { new ProjectAuthorizationRequirement(projectAuthorizationLevel) };
        var user = new ClaimsPrincipal();
        user.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, user, null);

        var stringId = Guid.Empty.ToString();
        Utils.Authorization.Project.MockHttpRequest(
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
            .Received(2)
            .HttpContext;
        await VerifyProjectProviderDidNotReceiveAny();

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(ProjectAuthorizationHandlerData.GetIsAllowed))]
    [ClassData(typeof(ProjectAuthorizationHandlerData.GetIsOwner))]
    internal async Task ProjectAuthorizationHandler_WhenProjectIsNull_ShouldSucceedForGlobalCustomFieldSetupsAndFailAsNotFoundForTheRest(
        ProjectAuthorizationLevel projectAuthorizationLevel,
        IdLocation requestLocation,
        Type? idType,
        Controller controller,
        Action requestAction,
        TargetKind stringIdType)
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
        Utils.Authorization.Project.MockHttpRequest(
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
            .Received(2)
            .HttpContext;
        await Utils.Authorization.Project.VerifyProjectProvider(_projectProvider, stringIdType, stringId);

        if (stringIdType == TargetKind.CustomFieldSetup)
        {
            // global custom field setups have no project, and the missing ones are found by the permission handler
            context.HasSucceeded.Should().BeTrue();
        }
        else
        {
            context.HasSucceeded.Should().BeFalse();
            context.HasFailed.Should().BeTrue();
            context.FailureReasons.OfType<NotFoundAuthorizationFailureReason>().Should().ContainSingle()
                .Which.Error.Should().Be(Utils.Authorization.GetNotFoundError(stringIdType));
        }
    }

    [Fact]
    public async Task
        ProjectAuthorizationHandler_WhenAuthorizationLevelIsNotSupported_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var requirements = new[] { new ProjectAuthorizationRequirement((ProjectAuthorizationLevel)int.MaxValue) };
        var user = UserFactory.Create();
        var userClaims = new ClaimsPrincipal();
        userClaims.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, userClaims, null);

        var project = ProjectFactory.Create();
        Utils.Authorization.Project.MockProjectProvider(_projectProvider, project);

        Utils.Authorization.Project.MockHttpRequest(
            _httpContextAccessor,
            IdLocation.Route,
            null,
            Controller.Project,
            Action.Get,
            project.Id.Value.ToString());

        // Act
        var outcome = async () => await _uut.HandleAsync(context);

        // Assert
        await outcome.Should().ThrowAsync<ArgumentOutOfRangeException>();

        context.HasSucceeded.Should().BeFalse();
    }

    [Theory]
    [ClassData(typeof(ProjectAuthorizationHandlerData.GetIsAllowed))]
    internal async Task ProjectAuthorizationHandler_WhenLevelIsIsAllowedAndProjectIsNotPrivate_ShouldSucceed(
        ProjectAuthorizationLevel projectAuthorizationLevel,
        IdLocation requestLocation,
        Type? idType,
        Controller controller,
        Action requestAction,
        TargetKind stringIdType)
    {
        // Arrange
        var requirements = new[] { new ProjectAuthorizationRequirement(projectAuthorizationLevel) };
        var user = UserFactory.Create();
        var userClaims = new ClaimsPrincipal();
        userClaims.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, userClaims, null);

        var project = ProjectFactory.Create(isPrivate: false);
        Utils.Authorization.Project.MockProjectProvider(_projectProvider, project);

        var stringId = Guid.NewGuid().ToString();
        Utils.Authorization.Project.MockHttpRequest(
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
            .Received(2)
            .HttpContext;
        await Utils.Authorization.Project.VerifyProjectProvider(_projectProvider, stringIdType, stringId);

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(ProjectAuthorizationHandlerData.GetIsAllowed))]
    [ClassData(typeof(ProjectAuthorizationHandlerData.GetIsOwner))]
    internal async Task ProjectAuthorizationHandler_WhenIsNotAllowed_ShouldFailAsNotFound(
        ProjectAuthorizationLevel projectAuthorizationLevel,
        IdLocation requestLocation,
        Type? idType,
        Controller controller,
        Action requestAction,
        TargetKind stringIdType)
    {
        // Arrange
        var requirements = new[] { new ProjectAuthorizationRequirement(projectAuthorizationLevel) };
        var user = UserFactory.Create();
        var userClaims = new ClaimsPrincipal();
        userClaims.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, userClaims, null);

        var project = ProjectFactory.Create(isPrivate: true);
        Utils.Authorization.Project.MockProjectProvider(_projectProvider, project);

        var stringId = project.Id.Value.ToString();
        Utils.Authorization.Project.MockHttpRequest(
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
            .Received(2)
            .HttpContext;
        await Utils.Authorization.Project.VerifyProjectProvider(_projectProvider, stringIdType, stringId);

        context.HasSucceeded.Should().BeFalse();
        context.HasFailed.Should().BeTrue();
        context.FailureReasons.OfType<NotFoundAuthorizationFailureReason>().Should().ContainSingle()
            .Which.Error.Should().Be(Utils.Authorization.GetNotFoundError(stringIdType));
    }

    [Theory]
    [ClassData(typeof(ProjectAuthorizationHandlerData.GetIsAllowed))]
    [ClassData(typeof(ProjectAuthorizationHandlerData.GetIsOwner))]
    internal async Task ProjectAuthorizationHandler_WhenIsAllowedMemberButNotOwner_ShouldSucceedOnlyForIsAllowedLevel(
        ProjectAuthorizationLevel projectAuthorizationLevel,
        IdLocation requestLocation,
        Type? idType,
        Controller controller,
        Action requestAction,
        TargetKind stringIdType)
    {
        // Arrange
        var requirements = new[] { new ProjectAuthorizationRequirement(projectAuthorizationLevel) };
        var user = UserFactory.Create();
        var userClaims = new ClaimsPrincipal();
        userClaims.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, userClaims, null);

        var project = ProjectFactory.Create(isPrivate: true);
        project.AddAllowedMember(user);
        Utils.Authorization.Project.MockProjectProvider(_projectProvider, project);

        var stringId = project.Id.Value.ToString();
        Utils.Authorization.Project.MockHttpRequest(
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
            .Received(2)
            .HttpContext;
        await Utils.Authorization.Project.VerifyProjectProvider(_projectProvider, stringIdType, stringId);

        context.HasSucceeded.Should().Be(projectAuthorizationLevel == ProjectAuthorizationLevel.IsAllowed);
        context.FailureReasons.Should().BeEmpty(); // the user knows that the project exists
    }

    [Theory]
    [ClassData(typeof(ProjectAuthorizationHandlerData.GetIsAllowed))]
    [ClassData(typeof(ProjectAuthorizationHandlerData.GetIsOwner))]
    internal async Task ProjectAuthorizationHandler_WhenIsAuthorized_ShouldSucceed(
        ProjectAuthorizationLevel projectAuthorizationLevel,
        IdLocation requestLocation,
        Type? idType,
        Controller controller,
        Action requestAction,
        TargetKind stringIdType)
    {
        // Arrange
        var requirements = new[] { new ProjectAuthorizationRequirement(projectAuthorizationLevel) };
        var user = UserFactory.Create();
        var userClaims = new ClaimsPrincipal();
        userClaims.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, userClaims, null);

        var project = ProjectFactory.Create(owner: user, isPrivate: true);
        project.AddAllowedMember(user);
        Utils.Authorization.Project.MockProjectProvider(_projectProvider, project);

        var stringId = project.Id.Value.ToString();
        Utils.Authorization.Project.MockHttpRequest(
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
            .Received(2)
            .HttpContext;
        await Utils.Authorization.Project.VerifyProjectProvider(_projectProvider, stringIdType, stringId);

        context.HasSucceeded.Should().BeTrue();
    }
}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using ConsiliumTempus.Common.UnitTests.Common.Entities;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using ConsiliumTempus.Infrastructure.Security.Authorization.WorkspaceAuthorization;
using ConsiliumTempus.Infrastructure.UnitTests.TestData.Security.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        var user = new ClaimsPrincipal();
        var context = new AuthorizationHandlerContext(requirements, user, null);

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

        context.HasSucceeded.Should().BeFalse();
    }

    [Fact]
    public async Task WorkspaceAuthorizationHandler_WhenRequestIsNull_ShouldFail()
    {
        // Arrange
        var requirements = new[] { new WorkspaceAuthorizationRequirement(WorkspaceAuthorizationLevel.IsCollaborator) };
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

        context.HasSucceeded.Should().BeFalse();
    }

    [Fact]
    public async Task WorkspaceAuthorizationHandler_WhenRequestIsEmpty_ShouldSucceed()
    {
        // Arrange
        var requirements = new[] { new WorkspaceAuthorizationRequirement(WorkspaceAuthorizationLevel.IsCollaborator) };
        var user = new ClaimsPrincipal();
        user.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, user, null);

        var bodyStream = JsonSerializer.SerializeToUtf8Bytes(new Dictionary<string, string>());
        _httpContextAccessor
            .HttpContext!
            .Request
            .Body
            .Returns(new MemoryStream(bodyStream));

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(2)
            .HttpContext;
        _workspaceProvider.DidNotReceive();

        context.HasSucceeded.Should().BeTrue();
    }

    [Fact]
    public async Task WorkspaceAuthorizationHandler_WhenRequestIdIsEmpty_ShouldSucceed()
    {
        // Arrange
        var requirements = new[] { new WorkspaceAuthorizationRequirement(WorkspaceAuthorizationLevel.IsCollaborator) };
        var user = new ClaimsPrincipal();
        user.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, user, null);

        const string stringId = "";
        var body = new Dictionary<string, string> { ["id"] = stringId };
        var bodyStream = JsonSerializer.SerializeToUtf8Bytes(body);
        _httpContextAccessor
            .HttpContext!
            .Request
            .Body
            .Returns(new MemoryStream(bodyStream));

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(2)
            .HttpContext;
        _workspaceProvider.DidNotReceive();

        context.HasSucceeded.Should().BeTrue();
    }

    [Fact]
    public async Task WorkspaceAuthorizationHandler_WhenRequestIdIsNotGuid_ShouldSucceed()
    {
        // Arrange
        var requirements = new[] { new WorkspaceAuthorizationRequirement(WorkspaceAuthorizationLevel.IsCollaborator) };
        var user = new ClaimsPrincipal();
        user.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, user, null);

        const string stringId = "not guid";
        var body = new Dictionary<string, string> { ["id"] = stringId };
        var bodyStream = JsonSerializer.SerializeToUtf8Bytes(body);
        _httpContextAccessor
            .HttpContext!
            .Request
            .Body
            .Returns(new MemoryStream(bodyStream));

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(2)
            .HttpContext;
        _workspaceProvider.DidNotReceive();

        context.HasSucceeded.Should().BeTrue();
    }

    [Fact]
    public async Task WorkspaceAuthorizationHandler_WhenWorkspaceIsNull_ShouldSucceed()
    {
        // Arrange
        var requirements = new[] { new WorkspaceAuthorizationRequirement(WorkspaceAuthorizationLevel.IsCollaborator) };
        var user = new ClaimsPrincipal();
        user.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, user, null);

        var stringId = Guid.NewGuid().ToString();
        var body = new Dictionary<string, string> { ["id"] = stringId };
        var bodyStream = JsonSerializer.SerializeToUtf8Bytes(body);
        _httpContextAccessor
            .HttpContext!
            .Request
            .Body
            .Returns(new MemoryStream(bodyStream));

        _workspaceProvider
            .GetWithMemberships(Arg.Any<WorkspaceId>())
            .ReturnsNull();

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(2)
            .HttpContext;
        await _workspaceProvider
            .Received(1)
            .GetWithMemberships(Arg.Is<WorkspaceId>(wId => wId.Value.ToString() == stringId));

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(WorkspaceAuthorizationHandlerData.GetAuthorizationLevels))]
    public async Task WorkspaceAuthorizationHandler_WhenNotAuthorized_ShouldFail(
        WorkspaceAuthorizationLevel workspaceAuthorizationLevel)
    {
        // Arrange
        var requirements = new[] { new WorkspaceAuthorizationRequirement(workspaceAuthorizationLevel) };
        var user = new ClaimsPrincipal();
        user.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, user, null);

        var stringId = Guid.NewGuid().ToString();
        var body = new Dictionary<string, string> { ["id"] = stringId };
        var bodyStream = JsonSerializer.SerializeToUtf8Bytes(body);
        _httpContextAccessor
            .HttpContext!
            .Request
            .Body
            .Returns(new MemoryStream(bodyStream));

        var workspace = WorkspaceFactory.Create();
        _workspaceProvider
            .GetWithMemberships(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(2)
            .HttpContext;
        await _workspaceProvider
            .Received(1)
            .GetWithMemberships(Arg.Is<WorkspaceId>(wId => wId.Value.ToString() == stringId));

        context.HasSucceeded.Should().BeFalse();
    }

    [Theory]
    [ClassData(typeof(WorkspaceAuthorizationHandlerData.GetAuthorizationLevels))]
    public async Task WorkspaceAuthorizationHandler_WhenIsAuthorized_ShouldSucceed(
        WorkspaceAuthorizationLevel workspaceAuthorizationLevel)
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
        var stringId = workspace.Id.ToString();
        var body = new Dictionary<string, string> { ["id"] = stringId };
        var bodyStream = JsonSerializer.SerializeToUtf8Bytes(body);
        _httpContextAccessor
            .HttpContext!
            .Request
            .Body
            .Returns(new MemoryStream(bodyStream));

        workspace.AddUserMembership(MembershipFactory.Create(user));
        _workspaceProvider
            .GetWithMemberships(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(2)
            .HttpContext;
        await _workspaceProvider
            .Received(1)
            .GetWithMemberships(Arg.Is<WorkspaceId>(wId => wId.Value.ToString() == stringId));

        context.HasSucceeded.Should().BeTrue();
    }
}
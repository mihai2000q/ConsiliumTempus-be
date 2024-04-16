using System.Security.Claims;
using System.Text.Json;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Infrastructure.Security.Authorization.Permission;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using ConsiliumTempus.Infrastructure.UnitTests.TestData.Security.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
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
    public async Task PermissionAuthHandler_WhenPermissionStringIsWrong_ShouldFail()
    {
        // Arrange
        var requirements = new[] { new PermissionRequirement("") };
        var user = new ClaimsPrincipal();
        var context = new AuthorizationHandlerContext(requirements, user, null);

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor.DidNotReceive();
        _workspaceProvider.DidNotReceive();
        _permissionProvider.DidNotReceive();

        context.HasSucceeded.Should().BeFalse();
    }

    [Fact]
    public async Task PermissionAuthHandler_WhenSubClaimIsNull_ShouldFail()
    {
        // Arrange
        var requirements = new[] { new PermissionRequirement(Permissions.ReadWorkspace.ToString()) };
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
    public async Task PermissionAuthHandler_WhenSubClaimIsNotGuid_ShouldFail()
    {
        // Arrange
        var requirements = new[] { new PermissionRequirement(Permissions.ReadWorkspace.ToString()) };
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
    public async Task PermissionAuthHandler_WhenRequestIsNull_ShouldFail()
    {
        // Arrange
        var requirements = new[] { new PermissionRequirement(Permissions.ReadWorkspace.ToString()) };
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
    public async Task PermissionAuthHandler_WhenRequestIsEmpty_ShouldSucceed(
        Permissions permission,
        PermissionAuthorizationHandlerData.RequestLocation requestLocation)
    {
        // Arrange
        var requirements = new[] { new PermissionRequirement(permission.ToString()) };
        var user = new ClaimsPrincipal();
        user.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, user, null);

        switch (requestLocation)
        {
            case PermissionAuthorizationHandlerData.RequestLocation.Route:
                _httpContextAccessor
                    .HttpContext!
                    .Request
                    .RouteValues
                    .Returns(new RouteValueDictionary());
                break;
            case PermissionAuthorizationHandlerData.RequestLocation.Query:
                _httpContextAccessor
                    .HttpContext!
                    .Request
                    .Query
                    .Returns(new QueryCollection());
                break;
            case PermissionAuthorizationHandlerData.RequestLocation.Body:
                var bodyStream = JsonSerializer.SerializeToUtf8Bytes(new Dictionary<string, string>());
                _httpContextAccessor
                    .HttpContext!
                    .Request
                    .Body
                    .Returns(new MemoryStream(bodyStream));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(requestLocation), requestLocation, null);
        }

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
    [ClassData(typeof(PermissionAuthorizationHandlerData.GetPermissionsWithId))]
    public async Task PermissionAuthHandler_WhenRequestIdIsEmpty_ShouldSucceed(
        Permissions permission,
        PermissionAuthorizationHandlerData.RequestLocation requestLocation,
        string? id)
    {
        // Arrange
        var requirements = new[] { new PermissionRequirement(permission.ToString()) };
        var user = new ClaimsPrincipal();
        user.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, user, null);

        const string stringId = "";
        switch (requestLocation)
        {
            case PermissionAuthorizationHandlerData.RequestLocation.Route:
                _httpContextAccessor
                    .HttpContext!
                    .Request
                    .RouteValues
                    .Returns(new RouteValueDictionary { [id ?? "id"] = stringId });
                break;
            case PermissionAuthorizationHandlerData.RequestLocation.Query:
                _httpContextAccessor
                    .HttpContext!
                    .Request
                    .Query
                    .Returns(new QueryCollection(
                        new Dictionary<string, StringValues> { [id ?? "id"] = stringId }));
                break;
            case PermissionAuthorizationHandlerData.RequestLocation.Body:
                var bodyStream = JsonSerializer.SerializeToUtf8Bytes(
                    new Dictionary<string, string> { [id ?? "id"] = stringId });
                _httpContextAccessor
                    .HttpContext!
                    .Request
                    .Body
                    .Returns(new MemoryStream(bodyStream));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(requestLocation), requestLocation, null);
        }

        _httpContextAccessor
            .HttpContext!
            .Request
            .RouteValues
            .Returns(new RouteValueDictionary { [id ?? "id"] = "" });

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(3)
            .HttpContext;
        _workspaceProvider.DidNotReceive();
        _permissionProvider.DidNotReceive();

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(PermissionAuthorizationHandlerData.GetPermissionsWithId))]
    public async Task PermissionAuthHandler_WhenRequestIdIsNotGuid_ShouldSucceed(
        Permissions permission,
        PermissionAuthorizationHandlerData.RequestLocation requestLocation,
        string? id)
    {
        // Arrange
        var requirements = new[] { new PermissionRequirement(permission.ToString()) };
        var user = new ClaimsPrincipal();
        user.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, user, null);

        const string stringId = "not guid";
        switch (requestLocation)
        {
            case PermissionAuthorizationHandlerData.RequestLocation.Route:
                _httpContextAccessor
                    .HttpContext!
                    .Request
                    .RouteValues
                    .Returns(new RouteValueDictionary { [id ?? "id"] = stringId });
                break;
            case PermissionAuthorizationHandlerData.RequestLocation.Query:
                _httpContextAccessor
                    .HttpContext!
                    .Request
                    .Query
                    .Returns(new QueryCollection(
                        new Dictionary<string, StringValues> { [id ?? "id"] = stringId }));
                break;
            case PermissionAuthorizationHandlerData.RequestLocation.Body:
                var bodyStream = JsonSerializer.SerializeToUtf8Bytes(
                    new Dictionary<string, string> { [id ?? "id"] = stringId });
                _httpContextAccessor
                    .HttpContext!
                    .Request
                    .Body
                    .Returns(new MemoryStream(bodyStream));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(requestLocation), requestLocation, null);
        }

        _httpContextAccessor
            .HttpContext!
            .Request
            .RouteValues
            .Returns(new RouteValueDictionary { [id ?? "id"] = "" });

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(3)
            .HttpContext;
        _workspaceProvider.DidNotReceive();
        _permissionProvider.DidNotReceive();

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(PermissionAuthorizationHandlerData.GetPermissionsWithIdAndType))]
    public async Task PermissionAuthHandler_WhenWorkspaceIsNull_ShouldSucceed(
        Permissions permission,
        PermissionAuthorizationHandlerData.RequestLocation requestLocation,
        string? id,
        PermissionAuthorizationHandlerData.StringIdType provider)
    {
        // Arrange
        var requirements = new[] { new PermissionRequirement(permission.ToString()) };
        var user = new ClaimsPrincipal();
        user.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
        ]));
        var context = new AuthorizationHandlerContext(requirements, user, null);

        var stringId = Guid.NewGuid().ToString();
        switch (requestLocation)
        {
            case PermissionAuthorizationHandlerData.RequestLocation.Body:
            {
                var body = new Dictionary<string, string> { [id ?? "id"] = stringId };
                var bodyStream = JsonSerializer.SerializeToUtf8Bytes(body);
                _httpContextAccessor
                    .HttpContext!
                    .Request
                    .Body
                    .Returns(new MemoryStream(bodyStream));
                break;
            }
            case PermissionAuthorizationHandlerData.RequestLocation.Route:
                _httpContextAccessor
                    .HttpContext!
                    .Request
                    .RouteValues
                    .Returns(new RouteValueDictionary { [id ?? "id"] = stringId });
                break;
            case PermissionAuthorizationHandlerData.RequestLocation.Query:
                _httpContextAccessor
                    .HttpContext!
                    .Request
                    .Query
                    .Returns(new QueryCollection(
                        new Dictionary<string, StringValues> { [id ?? "id"] = stringId }));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(requestLocation), requestLocation, null);
        }

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(2)
            .HttpContext;

        switch (provider)
        {
            case PermissionAuthorizationHandlerData.StringIdType.Workspace:
                await _workspaceProvider
                    .Received(1)
                    .Get(Arg.Is<WorkspaceId>(wId => wId.Value.ToString() == stringId));
                break;
            case PermissionAuthorizationHandlerData.StringIdType.Project:
                await _workspaceProvider
                    .Received(1)
                    .GetByProject(Arg.Is<ProjectId>(pId => pId.Value.ToString() == stringId));
                break;
            case PermissionAuthorizationHandlerData.StringIdType.ProjectSprint:
                await _workspaceProvider
                    .Received(1)
                    .GetByProjectSprint(Arg.Is<ProjectSprintId>(psId => psId.Value.ToString() == stringId));
                break;
        }

        _permissionProvider.DidNotReceive();

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(PermissionAuthorizationHandlerData.GetPermissionsWithIdAndType))]
    public async Task PermissionAuthHandler_WhenDoesNotHavePermission_ShouldFail(
        Permissions permission,
        PermissionAuthorizationHandlerData.RequestLocation requestLocation,
        string? id,
        PermissionAuthorizationHandlerData.StringIdType provider)
    {
        // Arrange
        var requirements = new[] { new PermissionRequirement(permission.ToString()) };
        var userId = Guid.NewGuid().ToString();
        var user = new ClaimsPrincipal();
        user.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, userId)
        ]));
        var context = new AuthorizationHandlerContext(requirements, user, null);

        var stringId = Guid.NewGuid().ToString();
        switch (requestLocation)
        {
            case PermissionAuthorizationHandlerData.RequestLocation.Body:
            {
                var body = new Dictionary<string, string> { [id ?? "id"] = stringId };
                var bodyStream = JsonSerializer.SerializeToUtf8Bytes(body);
                _httpContextAccessor
                    .HttpContext!
                    .Request
                    .Body
                    .Returns(new MemoryStream(bodyStream));
                break;
            }
            case PermissionAuthorizationHandlerData.RequestLocation.Route:
                _httpContextAccessor
                    .HttpContext!
                    .Request
                    .RouteValues
                    .Returns(new RouteValueDictionary { [id ?? "id"] = stringId });
                break;
            case PermissionAuthorizationHandlerData.RequestLocation.Query:
                _httpContextAccessor
                    .HttpContext!
                    .Request
                    .Query
                    .Returns(new QueryCollection(
                        new Dictionary<string, StringValues> { [id ?? "id"] = stringId }));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(requestLocation), requestLocation, null);
        }

        var workspace = WorkspaceFactory.Create();
        _workspaceProvider
            .Get(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        _workspaceProvider
            .GetByProject(Arg.Any<ProjectId>())
            .Returns(workspace);

        _workspaceProvider
            .GetByProjectSprint(Arg.Any<ProjectSprintId>())
            .Returns(workspace);

        _permissionProvider
            .GetPermissions(Arg.Any<UserId>(), Arg.Any<WorkspaceId>())
            .Returns([]);

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(2)
            .HttpContext;

        switch (provider)
        {
            case PermissionAuthorizationHandlerData.StringIdType.Workspace:
                await _workspaceProvider
                    .Received(1)
                    .Get(Arg.Is<WorkspaceId>(wId => wId.Value.ToString() == stringId));
                break;
            case PermissionAuthorizationHandlerData.StringIdType.Project:
                await _workspaceProvider
                    .Received(1)
                    .GetByProject(Arg.Is<ProjectId>(pId => pId.Value.ToString() == stringId));
                break;
            case PermissionAuthorizationHandlerData.StringIdType.ProjectSprint:
                await _workspaceProvider
                    .Received(1)
                    .GetByProjectSprint(Arg.Is<ProjectSprintId>(psId => psId.Value.ToString() == stringId));
                break;
            default: throw new Exception();
        }

        await _permissionProvider
            .Received(1)
            .GetPermissions(
                Arg.Is<UserId>(uId => uId.Value.ToString() == userId),
                Arg.Is<WorkspaceId>(wId => wId == workspace.Id));

        context.HasSucceeded.Should().BeFalse();
    }

    [Theory]
    [ClassData(typeof(PermissionAuthorizationHandlerData.GetPermissionsWithIdAndType))]
    public async Task PermissionAuthHandler_WhenTheyHavePermission_ShouldSucceed(
        Permissions permission,
        PermissionAuthorizationHandlerData.RequestLocation requestLocation,
        string? id,
        PermissionAuthorizationHandlerData.StringIdType provider)
    {
        // Arrange
        var requirements = new[] { new PermissionRequirement(permission.ToString()) };
        var userId = Guid.NewGuid().ToString();
        var user = new ClaimsPrincipal();
        user.AddIdentity(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, userId)
        ]));
        var context = new AuthorizationHandlerContext(requirements, user, null);

        var stringId = Guid.NewGuid().ToString();
        switch (requestLocation)
        {
            case PermissionAuthorizationHandlerData.RequestLocation.Body:
            {
                var body = new Dictionary<string, string> { [id ?? "id"] = stringId };
                var bodyStream = JsonSerializer.SerializeToUtf8Bytes(body);
                _httpContextAccessor
                    .HttpContext!
                    .Request
                    .Body
                    .Returns(new MemoryStream(bodyStream));
                break;
            }
            case PermissionAuthorizationHandlerData.RequestLocation.Route:
                _httpContextAccessor
                    .HttpContext!
                    .Request
                    .RouteValues
                    .Returns(new RouteValueDictionary { [id ?? "id"] = stringId });
                break;
            case PermissionAuthorizationHandlerData.RequestLocation.Query:
                _httpContextAccessor
                    .HttpContext!
                    .Request
                    .Query
                    .Returns(new QueryCollection(
                        new Dictionary<string, StringValues> { [id ?? "id"] = stringId }));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(requestLocation), requestLocation, null);
        }

        var workspace = WorkspaceFactory.Create();
        _workspaceProvider
            .Get(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        _workspaceProvider
            .GetByProject(Arg.Any<ProjectId>())
            .Returns(workspace);

        _workspaceProvider
            .GetByProjectSprint(Arg.Any<ProjectSprintId>())
            .Returns(workspace);

        _permissionProvider
            .GetPermissions(Arg.Any<UserId>(), Arg.Any<WorkspaceId>())
            .Returns([permission.ToString()]);

        // Act
        await _uut.HandleAsync(context);

        // Assert
        _ = _httpContextAccessor
            .Received(2)
            .HttpContext;

        switch (provider)
        {
            case PermissionAuthorizationHandlerData.StringIdType.Workspace:
                await _workspaceProvider
                    .Received(1)
                    .Get(Arg.Is<WorkspaceId>(wId => wId.Value.ToString() == stringId));
                break;
            case PermissionAuthorizationHandlerData.StringIdType.Project:
                await _workspaceProvider
                    .Received(1)
                    .GetByProject(Arg.Is<ProjectId>(pId => pId.Value.ToString() == stringId));
                break;
            case PermissionAuthorizationHandlerData.StringIdType.ProjectSprint:
                await _workspaceProvider
                    .Received(1)
                    .GetByProjectSprint(Arg.Is<ProjectSprintId>(psId => psId.Value.ToString() == stringId));
                break;
            default: throw new Exception();
        }

        await _permissionProvider
            .Received(1)
            .GetPermissions(
                Arg.Is<UserId>(uId => uId.Value.ToString() == userId),
                Arg.Is<WorkspaceId>(wId => wId == workspace.Id));

        context.HasSucceeded.Should().BeTrue();
    }
}
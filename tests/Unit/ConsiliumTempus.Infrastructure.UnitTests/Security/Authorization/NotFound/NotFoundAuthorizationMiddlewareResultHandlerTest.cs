using System.Text.Json;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Infrastructure.Security.Authorization.NotFound;
using ConsiliumTempus.Infrastructure.Security.Authorization.Permission;
using ConsiliumTempus.Infrastructure.Security.Authorization.ProjectAuthorization;
using ConsiliumTempus.Infrastructure.Security.Authorization.WorkspaceAuthorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace ConsiliumTempus.Infrastructure.UnitTests.Security.Authorization.NotFound;

public class NotFoundAuthorizationMiddlewareResultHandlerTest
{
    #region Setup

    private readonly IAuthenticationService _authenticationService;
    private readonly DefaultHttpContext _httpContext;
    private readonly AuthorizationPolicy _policy;
    private readonly NotFoundAuthorizationMiddlewareResultHandler _uut;

    private readonly IAuthorizationHandler _permissionHandler;
    private readonly IAuthorizationHandler _workspaceHandler;
    private readonly IAuthorizationHandler _projectHandler;

    private bool _nextWasCalled;

    public NotFoundAuthorizationMiddlewareResultHandlerTest()
    {
        var scopeFactory = Substitute.For<IServiceScopeFactory>();
        _permissionHandler = new PermissionAuthorizationHandler(scopeFactory);
        _workspaceHandler = new WorkspaceAuthorizationHandler(scopeFactory);
        _projectHandler = new ProjectAuthorizationHandler(scopeFactory);

        _authenticationService = Substitute.For<IAuthenticationService>();

        var problemDetailsFactory = Substitute.For<ProblemDetailsFactory>();
        problemDetailsFactory
            .CreateProblemDetails(
                Arg.Any<HttpContext>(),
                Arg.Any<int?>(),
                Arg.Any<string?>(),
                Arg.Any<string?>(),
                Arg.Any<string?>(),
                Arg.Any<string?>())
            .Returns(callInfo => new ProblemDetails
            {
                Status = callInfo.ArgAt<int?>(1),
                Title = callInfo.ArgAt<string?>(2)
            });

        var services = new ServiceCollection()
            .AddSingleton(_authenticationService)
            .AddSingleton(problemDetailsFactory)
            .BuildServiceProvider();

        _httpContext = new DefaultHttpContext { RequestServices = services };
        _httpContext.Response.Body = new MemoryStream();

        _policy = new AuthorizationPolicyBuilder().RequireAssertion(_ => true).Build();
        _uut = new NotFoundAuthorizationMiddlewareResultHandler();
    }

    private Task Handle(PolicyAuthorizationResult authorizeResult)
    {
        return _uut.HandleAsync(
            _ =>
            {
                _nextWasCalled = true;
                return Task.CompletedTask;
            },
            _httpContext,
            _policy,
            authorizeResult);
    }

    private static PolicyAuthorizationResult Forbidden(params AuthorizationFailureReason[] reasons)
    {
        return PolicyAuthorizationResult.Forbid(AuthorizationFailure.Failed(reasons));
    }

    private async Task<ProblemDetails> ReadProblemDetails()
    {
        _httpContext.Response.Body.Position = 0;
        return (await JsonSerializer.DeserializeAsync<ProblemDetails>(
            _httpContext.Response.Body,
            new JsonSerializerOptions(JsonSerializerDefaults.Web)))!;
    }

    #endregion

    [Fact]
    public async Task HandleAsync_WhenSucceeded_ShouldCallNext()
    {
        // Arrange - nothing
        
        // Act
        await Handle(PolicyAuthorizationResult.Success());

        // Assert
        _nextWasCalled.Should().BeTrue();
        _httpContext.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task HandleAsync_WhenForbiddenWithoutNotFoundReason_ShouldForbid()
    {
        // Arrange
        var reason = new AuthorizationFailureReason(_permissionHandler, "no permission");

        // Act
        await Handle(Forbidden(reason));

        // Assert
        _nextWasCalled.Should().BeFalse();
        await _authenticationService
            .Received(1)
            .ForbidAsync(_httpContext, Arg.Any<string?>(), Arg.Any<AuthenticationProperties?>());
        _httpContext.Response.Body.Length.Should().Be(0);
    }

    [Fact]
    public async Task HandleAsync_WhenForbiddenWithNotFoundReason_ShouldReturnNotFoundProblemDetails()
    {
        // Arrange
        var reason = new NotFoundAuthorizationFailureReason(_permissionHandler, Errors.Project.NotFound);

        // Act
        await Handle(Forbidden(reason));

        // Assert
        _nextWasCalled.Should().BeFalse();
        await _authenticationService
            .DidNotReceive()
            .ForbidAsync(Arg.Any<HttpContext>(), Arg.Any<string?>(), Arg.Any<AuthenticationProperties?>());

        _httpContext.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        _httpContext.Response.ContentType.Should().Be("application/problem+json");
        _httpContext.Items["Errors"].Should().BeEquivalentTo(new List<ErrorOr.Error> { Errors.Project.NotFound });

        var problemDetails = await ReadProblemDetails();
        problemDetails.Status.Should().Be(StatusCodes.Status404NotFound);
        problemDetails.Title.Should().Be(Errors.Project.NotFound.Description);
    }

    [Fact]
    public async Task HandleAsync_WhenForbiddenWithNotFoundReasonAndOtherReasons_ShouldReturnNotFound()
    {
        // Arrange
        var otherReason = new AuthorizationFailureReason(_permissionHandler, "no permission");
        var reason = new NotFoundAuthorizationFailureReason(_projectHandler, Errors.ProjectTask.NotFound);

        // Act
        await Handle(Forbidden(otherReason, reason));

        // Assert
        _httpContext.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        var problemDetails = await ReadProblemDetails();
        problemDetails.Title.Should().Be(Errors.ProjectTask.NotFound.Description);
    }

    [Fact]
    public async Task HandleAsync_WhenForbiddenWithSeveralNotFoundReasons_ShouldUseTheOutermostResource()
    {
        // Arrange
        var projectReason = new NotFoundAuthorizationFailureReason(_projectHandler, Errors.Project.NotFound);
        var workspaceReason = new NotFoundAuthorizationFailureReason(_workspaceHandler, Errors.Workspace.NotFound);
        var permissionReason =
            new NotFoundAuthorizationFailureReason(_permissionHandler, Errors.CustomFieldSetup.NotFound);

        // Act
        await Handle(Forbidden(projectReason, workspaceReason, permissionReason));

        // Assert
        var problemDetails = await ReadProblemDetails();
        problemDetails.Title.Should().Be(Errors.CustomFieldSetup.NotFound.Description);
    }

    [Fact]
    public async Task HandleAsync_WhenForbiddenWithWorkspaceAndProjectNotFoundReasons_ShouldUseTheWorkspace()
    {
        // Arrange
        var projectReason = new NotFoundAuthorizationFailureReason(_projectHandler, Errors.Project.NotFound);
        var workspaceReason = new NotFoundAuthorizationFailureReason(_workspaceHandler, Errors.Workspace.NotFound);

        // Act
        await Handle(Forbidden(projectReason, workspaceReason));

        // Assert
        var problemDetails = await ReadProblemDetails();
        problemDetails.Title.Should().Be(Errors.Workspace.NotFound.Description);
    }
}

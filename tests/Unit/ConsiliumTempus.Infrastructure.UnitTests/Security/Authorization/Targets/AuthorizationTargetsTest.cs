using ConsiliumTempus.Infrastructure.Security.Authorization.Targets;
using ConsiliumTempus.Infrastructure.UnitTests.TestData.Security.Authorization;
using ConsiliumTempus.Infrastructure.UnitTests.TestUtils;
using Microsoft.AspNetCore.Http;
using Action = ConsiliumTempus.Infrastructure.UnitTests.TestUtils.Utils.Authorization.Action;
using Controller = ConsiliumTempus.Infrastructure.UnitTests.TestUtils.Utils.Authorization.Controller;
using Method = ConsiliumTempus.Infrastructure.UnitTests.TestUtils.Utils.Authorization.Method;

namespace ConsiliumTempus.Infrastructure.UnitTests.Security.Authorization.Targets;

public class AuthorizationTargetsTest
{
    [Theory]
    [ClassData(typeof(AuthorizationTargetsData.GetOwnTargets))]
    internal void For_WhenEndpointIsOfItsControllerEntity_ShouldTargetTheEntityOfTheController(
        Controller controller,
        TargetKind kind)
    {
        // Arrange
        var request = Utils.Authorization.Targets.CreateRequest(controller, Action.Get, Method.GET);

        // Act
        var outcome = AuthorizationTargets.For(request);

        // Assert
        outcome.Should().Be(new TargetId(kind, IdLocation.Route));
        outcome.Property.Should().Be("id");
    }

    [Theory]
    [ClassData(typeof(AuthorizationTargetsData.GetHttpMethods))]
    internal void For_WhenEndpointIsOfItsControllerEntity_ShouldFindTheIdWhereTheHttpMethodPutsIt(
        Method method,
        IdLocation location)
    {
        // Arrange
        var request = Utils.Authorization.Targets.CreateRequest(Controller.Project, Action.Get, method);

        // Act
        var outcome = AuthorizationTargets.For(request);

        // Assert
        outcome.Location.Should().Be(location);
    }

    [Theory]
    [ClassData(typeof(AuthorizationTargetsData.GetParentTargets))]
    internal void For_WhenEndpointWorksThroughAParent_ShouldTargetTheParent(
        Controller controller,
        Action action,
        Method method,
        TargetKind kind,
        IdLocation location,
        string property)
    {
        // Arrange
        var request = Utils.Authorization.Targets.CreateRequest(controller, action, method);

        // Act
        var outcome = AuthorizationTargets.For(request);

        // Assert
        outcome.Should().Be(new TargetId(kind, location, Qualified: true));
        outcome.Property.Should().Be(property);
    }

    [Theory]
    [ClassData(typeof(AuthorizationTargetsData.GetProjectTargets))]
    internal void ForProject_WhenEndpointIsAboutTheProjectOfAnotherEntity_ShouldTargetTheProject(
        Action action,
        Method method,
        IdLocation location)
    {
        // Arrange
        var request = Utils.Authorization.Targets.CreateRequest(Controller.CustomFieldSetup, action, method);

        // Act
        var outcome = AuthorizationTargets.ForProject(request);

        // Assert
        outcome.Should().Be(new TargetId(TargetKind.Project, location, Qualified: true));
        outcome.Property.Should().Be("projectId");
        AuthorizationTargets.For(request).Kind.Should().Be(TargetKind.CustomFieldSetup);
    }

    [Fact]
    internal void ForProject_WhenEndpointHasNoTargetOfItsOwn_ShouldTargetTheSameAsTheRest()
    {
        // Arrange
        var request = Utils.Authorization.Targets.CreateRequest(Controller.ProjectSprint, Action.Create, Method.POST);

        // Act
        var outcome = AuthorizationTargets.ForProject(request);

        // Assert
        outcome.Should().Be(AuthorizationTargets.For(request));
    }

    [Theory]
    [ClassData(typeof(AuthorizationTargetsData.GetControllersWithoutTarget))]
    internal void For_WhenControllerHasNoTarget_ShouldThrow(string? controller)
    {
        // Arrange
        var request = Utils.Authorization.Targets.CreateRequest(controller, "Get", HttpMethods.Get);

        // Act
        var outcome = () => AuthorizationTargets.For(request);

        // Assert
        outcome.Should().Throw<InvalidOperationException>();
    }

    [Theory]
    [ClassData(typeof(AuthorizationTargetsData.GetNumbers))]
    internal void For_WhenControllerIsANumber_ShouldThrow(string controller)
    {
        // Arrange
        var request = Utils.Authorization.Targets.CreateRequest(controller, "Get", HttpMethods.Get);

        // Act
        var outcome = () => AuthorizationTargets.For(request);

        // Assert
        outcome.Should().Throw<InvalidOperationException>();
    }

    [Theory]
    [ClassData(typeof(AuthorizationTargetsData.GetNumbers))]
    internal void For_WhenActionIsANumber_ShouldTargetTheEntityOfTheController(string action)
    {
        // Arrange
        var request = Utils.Authorization.Targets.CreateRequest("Project", action, HttpMethods.Get);

        // Act
        var outcome = AuthorizationTargets.For(request);

        // Assert
        outcome.Should().Be(new TargetId(TargetKind.Project, IdLocation.Route));
    }

    [Theory]
    [ClassData(typeof(AuthorizationTargetsData.GetHttpMethodsWithoutTarget))]
    internal void For_WhenHttpMethodHasNoTarget_ShouldThrow(string method)
    {
        // Arrange
        var request = Utils.Authorization.Targets.CreateRequest("Project", "Update", method);

        // Act
        var outcome = () => AuthorizationTargets.For(request);

        // Assert
        outcome.Should().Throw<InvalidOperationException>();
    }
}

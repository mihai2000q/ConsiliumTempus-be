using System.Diagnostics;
using ConsiliumTempus.Api.Common.Errors;
using ConsiliumTempus.Api.Common.Http;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace ConsiliumTempus.Api.UnitTests.Common.Errors;

public class ConsiliumTempusProblemDetailsFactoryTest
{
    #region Setup

    private const string TraceId = "trace-id";

    private readonly DefaultHttpContext _httpContext;
    private readonly ConsiliumTempusProblemDetailsFactory _uut;

    public ConsiliumTempusProblemDetailsFactoryTest()
    {
        var options = new ApiBehaviorOptions();
        options.ClientErrorMapping[StatusCodes.Status404NotFound] = new ClientErrorData
        {
            Title = "Not Found",
            Link = "https://example.com/404"
        };

        _httpContext = new DefaultHttpContext { TraceIdentifier = TraceId };
        _uut = new ConsiliumTempusProblemDetailsFactory(Options.Create(options));
    }

    private static ModelStateDictionary CreateModelState()
    {
        var modelState = new ModelStateDictionary();
        modelState.AddModelError("Name", "Name is required");
        return modelState;
    }

    #endregion

    [Fact]
    public void CreateProblemDetails_WhenNothingIsProvided_ShouldDefaultToInternalServerError()
    {
        // Arrange - nothing

        // Act
        var outcome = _uut.CreateProblemDetails(_httpContext);

        // Assert
        outcome.Status.Should().Be(StatusCodes.Status500InternalServerError);
        outcome.Title.Should().BeNull();
        outcome.Type.Should().BeNull();
        outcome.Detail.Should().BeNull();
        outcome.Instance.Should().BeNull();
        outcome.Extensions["traceId"].Should().Be(TraceId);
        outcome.Extensions.Should().NotContainKey(ConsiliumTempusProblemDetailsFactory.ErrorCodes);
    }

    [Fact]
    public void CreateProblemDetails_WhenAllArgumentsAreProvided_ShouldUseThem()
    {
        // Arrange - nothing

        // Act
        var outcome = _uut.CreateProblemDetails(
            _httpContext,
            StatusCodes.Status409Conflict,
            "Title",
            "Type",
            "Detail",
            "Instance");

        // Assert
        outcome.Status.Should().Be(StatusCodes.Status409Conflict);
        outcome.Title.Should().Be("Title");
        outcome.Type.Should().Be("Type");
        outcome.Detail.Should().Be("Detail");
        outcome.Instance.Should().Be("Instance");
    }

    [Fact]
    public void CreateProblemDetails_WhenStatusHasClientErrorMapping_ShouldFillMissingTitleAndType()
    {
        // Arrange - nothing

        // Act
        var outcome = _uut.CreateProblemDetails(_httpContext, StatusCodes.Status404NotFound);

        // Assert
        outcome.Title.Should().Be("Not Found");
        outcome.Type.Should().Be("https://example.com/404");
    }

    [Fact]
    public void CreateProblemDetails_WhenTitleAndTypeAreProvided_ShouldNotOverrideThemWithClientErrorMapping()
    {
        // Arrange - nothing

        // Act
        var outcome = _uut.CreateProblemDetails(
            _httpContext,
            StatusCodes.Status404NotFound,
            "Custom Title",
            "Custom Type");

        // Assert
        outcome.Title.Should().Be("Custom Title");
        outcome.Type.Should().Be("Custom Type");
    }

    [Fact]
    public void CreateProblemDetails_WhenThereIsAnActivity_ShouldUseItsIdAsTraceId()
    {
        // Arrange
        using var activity = new Activity("test").Start();

        // Act
        var outcome = _uut.CreateProblemDetails(_httpContext);

        // Assert
        outcome.Extensions["traceId"].Should().Be(activity.Id);
    }

    [Fact]
    public void CreateProblemDetails_WhenErrorsAreInHttpContext_ShouldAddTheirCodes()
    {
        // Arrange
        _httpContext.Items[HttpContextItemKeys.Errors] = new List<Error>
        {
            Error.NotFound("Project.NotFound", "Project not found"),
            Error.Conflict("Project.Conflict", "Project conflict")
        };

        // Act
        var outcome = _uut.CreateProblemDetails(_httpContext, StatusCodes.Status404NotFound);

        // Assert
        outcome.Extensions[ConsiliumTempusProblemDetailsFactory.ErrorCodes]
            .Should().BeAssignableTo<IEnumerable<string>>()
            .Which.Should().Equal("Project.NotFound", "Project.Conflict");
    }

    [Fact]
    public void CreateValidationProblemDetails_WhenNothingIsProvided_ShouldDefaultToBadRequest()
    {
        // Arrange - nothing

        // Act
        var outcome = _uut.CreateValidationProblemDetails(_httpContext, CreateModelState());

        // Assert
        outcome.Status.Should().Be(StatusCodes.Status400BadRequest);
        outcome.Title.Should().Be("One or more validation errors occurred.");
        outcome.Errors.Should().ContainSingle()
            .Which.Should().Match<KeyValuePair<string, string[]>>(e =>
                e.Key == "Name" && e.Value.SequenceEqual(new[] { "Name is required" }));
        outcome.Extensions["traceId"].Should().Be(TraceId);
        outcome.Extensions.Should().NotContainKey(ConsiliumTempusProblemDetailsFactory.ErrorCodes);
    }

    [Fact]
    public void CreateValidationProblemDetails_WhenAllArgumentsAreProvided_ShouldUseThem()
    {
        // Arrange - nothing

        // Act
        var outcome = _uut.CreateValidationProblemDetails(
            _httpContext,
            CreateModelState(),
            StatusCodes.Status422UnprocessableEntity,
            "Custom Title",
            "Type",
            "Detail",
            "Instance");

        // Assert
        outcome.Status.Should().Be(StatusCodes.Status422UnprocessableEntity);
        outcome.Title.Should().Be("Custom Title");
        outcome.Type.Should().Be("Type");
        outcome.Detail.Should().Be("Detail");
        outcome.Instance.Should().Be("Instance");
    }
}

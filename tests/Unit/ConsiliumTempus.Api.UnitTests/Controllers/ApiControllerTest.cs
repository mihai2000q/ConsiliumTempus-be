using ConsiliumTempus.Api.Common.Errors;
using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.Api.UnitTests.TestData;
using ErrorOr;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ConsiliumTempus.Api.UnitTests.Controllers;

public class ApiControllerTest
{
    #region Setup

    private readonly TestController _uut;

    public ApiControllerTest()
    {
        _uut = new TestController(Substitute.For<IMapper>(), Substitute.For<ISender>());

        var services = new ServiceCollection()
            .AddSingleton<ProblemDetailsFactory>(
                new ConsiliumTempusProblemDetailsFactory(Options.Create(new ApiBehaviorOptions())))
            .BuildServiceProvider();
        _uut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { RequestServices = services }
        };
    }

    private sealed class TestController(IMapper mapper, ISender mediator) : ApiController(mapper, mediator)
    {
        public IActionResult Fail(List<Error> errors) => Problem(errors);
    }

    private static IEnumerable<string> ErrorCodes(ProblemDetails problemDetails)
    {
        return (IEnumerable<string>)problemDetails.Extensions[ConsiliumTempusProblemDetailsFactory.ErrorCodes]!;
    }

    #endregion

    [Fact]
    public void Problem_WhenThereAreNoErrors_ShouldReturnInternalServerError()
    {
        // Arrange - nothing

        // Act
        var outcome = _uut.Fail([]);

        // Assert
        var result = outcome.Should().BeAssignableTo<ObjectResult>().Subject;
        var problemDetails = result.Value.Should().BeOfType<ProblemDetails>().Subject;
        problemDetails.Status.Should().Be(StatusCodes.Status500InternalServerError);
        problemDetails.Extensions.Should().NotContainKey(ConsiliumTempusProblemDetailsFactory.ErrorCodes);
    }

    [Theory]
    [ClassData(typeof(ApiControllerData.GetSingleErrors))]
    public void Problem_WhenThereIsOneError_ShouldMapItsTypeToStatusCode(Error error, int statusCode)
    {
        // Arrange - parameterized

        // Act
        var outcome = _uut.Fail([error]);

        // Assert
        var result = outcome.Should().BeAssignableTo<ObjectResult>().Subject;
        result.StatusCode.Should().Be(statusCode);

        var problemDetails = result.Value.Should().BeOfType<ProblemDetails>().Subject;
        problemDetails.Status.Should().Be(statusCode);
        problemDetails.Title.Should().Be(error.Description);
        ErrorCodes(problemDetails).Should().Equal(error.Code);
    }

    [Fact]
    public void Problem_WhenAllErrorsAreValidation_ShouldReturnValidationProblemWithTheirDescriptions()
    {
        // Arrange
        var errors = new List<Error>
        {
            Error.Validation("Name", "Name is required"),
            Error.Validation("Name", "Name is too long"),
            Error.Validation("Description", "Description is required")
        };

        // Act
        var outcome = _uut.Fail(errors);

        // Assert
        var result = outcome.Should().BeAssignableTo<ObjectResult>().Subject;
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

        var problemDetails = result.Value.Should().BeOfType<ValidationProblemDetails>().Subject;
        problemDetails.Status.Should().Be(StatusCodes.Status400BadRequest);
        problemDetails.Errors.Keys.Should().BeEquivalentTo("Name", "Description");
        problemDetails.Errors["Name"].Should().Equal("Name is required", "Name is too long");
        problemDetails.Errors["Description"].Should().Equal("Description is required");
    }

    [Theory]
    [ClassData(typeof(ApiControllerData.GetMixedErrors))]
    public void Problem_WhenValidationErrorsAreMixedWithOtherErrors_ShouldUseTheFirstErrorAndListAllCodes(
        List<Error> errors,
        int statusCode)
    {
        // Arrange - parameterized

        // Act
        var outcome = _uut.Fail(errors);

        // Assert
        var result = outcome.Should().BeAssignableTo<ObjectResult>().Subject;
        result.StatusCode.Should().Be(statusCode);

        var problemDetails = result.Value.Should().BeOfType<ProblemDetails>().Subject;
        problemDetails.Status.Should().Be(statusCode);
        problemDetails.Title.Should().Be(errors[0].Description);
        ErrorCodes(problemDetails).Should().Equal(errors.Select(e => e.Code));
    }
}

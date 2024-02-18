using ConsiliumTempus.Api.Controllers;
using ErrorOr;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Api.UnitTests.TestUtils;

internal static partial class Utils
{
    private static readonly Dictionary<ErrorType, int> StatusCodesMap = new()
    {
        { ErrorType.Unauthorized, StatusCodes.Status401Unauthorized },
        { ErrorType.NotFound, StatusCodes.Status404NotFound },
        { ErrorType.Conflict, StatusCodes.Status409Conflict }
    };

    internal static IMapper GetMapper<TMappingConfig>()
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(TMappingConfig).Assembly);
        return new Mapper(config);
    }

    internal static HttpContext ResolveHttpContext(ApiController controller)
    {
        var httpContext = Substitute.For<HttpContext>();
        controller.ControllerContext.HttpContext = httpContext;
        httpContext
            .Items
            .Returns(new Dictionary<object, object?>());
        httpContext
            .RequestServices
            .ReturnsNull();
        return httpContext;
    }

    internal static void ValidateError(this IActionResult response, Error expectedError)
    {
        response.Should().BeOfType<ObjectResult>();
        ((ObjectResult)response).Value.Should().BeOfType<ProblemDetails>();

        var error = ((ObjectResult)response).Value as ProblemDetails;
        error?.Status.Should().Be(StatusCodesMap[expectedError.Type]);
        error?.Title.Should().Be(expectedError.Description);
    }
}
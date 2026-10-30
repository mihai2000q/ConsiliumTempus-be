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
        { ErrorType.Forbidden, StatusCodes.Status403Forbidden },
        { ErrorType.NotFound, StatusCodes.Status404NotFound },
        { ErrorType.Conflict, StatusCodes.Status409Conflict },
        { ErrorType.Failure, StatusCodes.Status500InternalServerError },
        { ErrorType.Unexpected, StatusCodes.Status500InternalServerError }
    };

    // Scanning the global config is not thread safe once it has been used, so it is done only once for all tests
    private static readonly Lazy<IMapper> SharedMapper = new(() =>
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(ApiController).Assembly);
        return new Mapper(config);
    });

    internal static IMapper GetMapper<TMappingConfig>() => SharedMapper.Value;

    internal static void ResolveHttpContext(ApiController controller)
    {
        var httpContext = Substitute.For<HttpContext>();
        controller.ControllerContext.HttpContext = httpContext;
        httpContext
            .Items
            .Returns(new Dictionary<object, object?>());
        httpContext
            .RequestServices
            .ReturnsNull();
    }

    extension(IActionResult response)
    {
        internal void ValidateError(Error expectedError)
        {
            response.Should().BeOfType<ObjectResult>();
            ((ObjectResult)response).Value.Should().BeOfType<ProblemDetails>();

            var error = (ProblemDetails)((ObjectResult)response).Value!;
            error.Status.Should().Be(StatusCodesMap[expectedError.Type]);
            error.Title.Should().Be(expectedError.Description);
        }

        internal T ToResponse<T>()
        {
            response.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)response).Value.Should().BeAssignableTo<T>();

            return (T)((OkObjectResult)response).Value!;
        }
    }
}
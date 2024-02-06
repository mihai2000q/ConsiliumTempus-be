using ConsiliumTempus.Api.Controllers;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.UnitTests.TestUtils;

public static partial class Utils
{
    public static IMapper GetMapper<TMappingConfig>()
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(TMappingConfig).Assembly);
        return new Mapper(config);
    }
    
    public static Mock<HttpContext> ResolveHttpContext(ApiController controller)
    {
        Mock<HttpContext> httpContext = new();
        controller.ControllerContext.HttpContext = httpContext.Object;
        httpContext.SetupGet(h => h.Items)
            .Returns(new Dictionary<object, object?>());
        return httpContext;
    }

    public static void ValidateError(this IActionResult response, int statusCode, string title)
    {
        response.Should().BeOfType<ObjectResult>();
        ((ObjectResult)response).Value.Should().BeOfType<ProblemDetails>();
        
        var error = ((ObjectResult)response).Value as ProblemDetails;
        error?.Status.Should().Be(statusCode);
        error?.Title.Should().Be(title);
    }
}
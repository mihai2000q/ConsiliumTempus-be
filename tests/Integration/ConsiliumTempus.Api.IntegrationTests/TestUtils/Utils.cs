using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.IntegrationTests.TestUtils;

public static partial class Utils
{
    private static readonly Dictionary<HttpStatusCode, int> StatusCodesMap = new()
    {
        { HttpStatusCode.Unauthorized, StatusCodes.Status401Unauthorized },
        { HttpStatusCode.NotFound, StatusCodes.Status404NotFound },
        { HttpStatusCode.Conflict, StatusCodes.Status409Conflict }
    };
    
    public static async Task ValidateError(this HttpResponseMessage response, HttpStatusCode statusCode, string title)
    {
        response.StatusCode.Should().Be(statusCode);

        var error = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        error?.Title.Should().Be(title);
        error?.Status.Should().Be(StatusCodesMap[statusCode]);
        var errorCodes = error?.Extensions["errorCodes"] as JsonElement?;
        errorCodes?.ValueKind.Should().Be(JsonValueKind.Array);
    }
}
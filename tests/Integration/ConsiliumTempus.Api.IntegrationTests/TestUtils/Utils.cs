using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ErrorOr;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.IntegrationTests.TestUtils;

internal static partial class Utils
{
    private static readonly Dictionary<ErrorType, HttpStatusCode> ErrorCodeToHttpStatusMap = new()
    {
        { ErrorType.Unauthorized, HttpStatusCode.Unauthorized },
        { ErrorType.NotFound, HttpStatusCode.NotFound },
        { ErrorType.Conflict, HttpStatusCode.Conflict }
    };
    
    private static readonly Dictionary<HttpStatusCode, int> StatusCodesMap = new()
    {
        { HttpStatusCode.Unauthorized, StatusCodes.Status401Unauthorized },
        { HttpStatusCode.NotFound, StatusCodes.Status404NotFound },
        { HttpStatusCode.Conflict, StatusCodes.Status409Conflict }
    };

    internal static async Task ValidateError(this HttpResponseMessage response, Error domainError)
    {
        var statusCode = ErrorCodeToHttpStatusMap[domainError.Type];
        response.StatusCode.Should().Be(statusCode);

        var error = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        error?.Title.Should().Be(domainError.Description);
        error?.Status.Should().Be(StatusCodesMap[statusCode]);
        var errorCodes = error?.Extensions["errorCodes"] as JsonElement?;
        errorCodes?.ValueKind.Should().Be(JsonValueKind.Array);
    }

    internal static async Task ValidateValidationErrors(this HttpResponseMessage response)
    {
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var error = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        error?.Status.Should().Be(StatusCodes.Status400BadRequest);
        var errorCodes = error?.Extensions["errors"] as JsonElement?;
        errorCodes?.ValueKind.Should().Be(JsonValueKind.Object);
    }
}
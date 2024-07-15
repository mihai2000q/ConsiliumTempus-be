using System.Net.Http.Json;
using System.Text.Json;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.User;
using ErrorOr;
using FluentAssertions.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.IntegrationTests.TestUtils;

internal static partial class Utils
{
    internal static readonly TimeSpan TimeSpanPrecision = 1.Minutes();

    private static readonly Dictionary<ErrorType, HttpStatusCode> ErrorCodeToHttpStatusMap = new()
    {
        { ErrorType.Validation, HttpStatusCode.BadRequest },
        { ErrorType.Unauthorized, HttpStatusCode.Unauthorized },
        { ErrorType.NotFound, HttpStatusCode.NotFound },
        { ErrorType.Conflict, HttpStatusCode.Conflict }
    };

    private static readonly Dictionary<HttpStatusCode, int> StatusCodesMap = new()
    {
        { HttpStatusCode.BadRequest, StatusCodes.Status400BadRequest },
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

    internal static void ShouldBeOrdered(this IReadOnlyList<ProjectTaskAggregate> tasks)
    {
        var customOrderPosition = 0;
        tasks.Should().AllSatisfy(t =>
            t.CustomOrderPosition.Value.Should().Be(customOrderPosition++));
    }

    internal static void ShouldBeOrdered(this IReadOnlyList<ProjectStage> stages)
    {
        var customOrderPosition = 0;
        stages.Should().AllSatisfy(s =>
            s.CustomOrderPosition.Value.Should().Be(customOrderPosition++));
    }

    internal static void ShouldBeCreated(this Audit audit, UserAggregate createdBy)
    {
        audit.CreatedBy.Should().Be(createdBy);
        audit.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        audit.UpdatedBy.Should().Be(createdBy);
        audit.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
    }

    internal static void ShouldBeUpdated(this Audit audit, UserAggregate updatedBy)
    {
        audit.UpdatedBy.Should().Be(updatedBy);
        audit.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
    }

    internal static string ToOrderByQueryParam(this string[] orderBy)
    {
        return string.Join('&', orderBy.Select(o => $"orderBy={o}"));
    }

    internal static string ToSearchQueryParam(this string[] search)
    {
        return string.Join('&', search.Select(s => $"search={s}"));
    }
}
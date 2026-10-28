using System.Net.Http.Json;
using System.Text.Json;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.ValueObjects;
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
        { ErrorType.Conflict, HttpStatusCode.Conflict },
        { ErrorType.Forbidden, HttpStatusCode.Forbidden }
    };

    private static readonly Dictionary<HttpStatusCode, int> StatusCodesMap = new()
    {
        { HttpStatusCode.BadRequest, StatusCodes.Status400BadRequest },
        { HttpStatusCode.Unauthorized, StatusCodes.Status401Unauthorized },
        { HttpStatusCode.Forbidden, StatusCodes.Status403Forbidden },
        { HttpStatusCode.NotFound, StatusCodes.Status404NotFound },
        { HttpStatusCode.Conflict, StatusCodes.Status409Conflict }
    };

    extension(HttpResponseMessage response)
    {
        internal async Task ValidateError(Error domainError)
        {
            var statusCode = ErrorCodeToHttpStatusMap[domainError.Type];
            response.StatusCode.Should().Be(statusCode);

            var error = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            error.Should().NotBeNull();
            error!.Title.Should().Be(domainError.Description);
            error.Status.Should().Be(StatusCodesMap[statusCode]);
            var errorCodes = error.Extensions["errorCodes"] as JsonElement?;
            errorCodes.Should().NotBeNull();
            errorCodes!.Value.ValueKind.Should().Be(JsonValueKind.Array);
        }

        internal async Task ValidateValidationErrors()
        {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var error = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            error.Should().NotBeNull();
            error!.Status.Should().Be(StatusCodes.Status400BadRequest);
            var errors = error.Extensions["errors"] as JsonElement?;
            errors.Should().NotBeNull();
            errors!.Value.ValueKind.Should().Be(JsonValueKind.Object);
        }
    }

    internal static void ShouldBeOrdered(this IReadOnlyList<ProjectTaskAggregate> tasks)
    {
        var customOrderPosition = 0;
        tasks.Should().AllSatisfyOrEmpty(t =>
            t.CustomOrderPosition.Value.Should().Be(customOrderPosition++));
    }

    internal static void ShouldBeOrdered(this IReadOnlyList<ProjectStage> stages)
    {
        var customOrderPosition = 0;
        stages.Should().AllSatisfyOrEmpty(s =>
            s.CustomOrderPosition.Value.Should().Be(customOrderPosition++));
    }

    internal static void ShouldBeOrdered(this IReadOnlyList<SingleSelectOption> options)
    {
        var customOrderPosition = 0;
        options.OrderBy(s => s.CustomOrderPosition)
            .Should().AllSatisfyOrEmpty(o => o.CustomOrderPosition.Value.Should().Be(customOrderPosition++));
    }

    internal static void ShouldBeOrdered(this IReadOnlyList<MultiSelectOption> options)
    {
        var customOrderPosition = 0;
        options.OrderBy(s => s.CustomOrderPosition)
            .Should().AllSatisfyOrEmpty(o => o.CustomOrderPosition.Value.Should().Be(customOrderPosition++));
    }

    extension(Audit audit)
    {
        internal void ShouldBeCreated(UserAggregate createdBy)
        {
            audit.CreatedBy.Should().Be(createdBy);
            audit.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            audit.UpdatedBy.Should().Be(createdBy);
            audit.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal void ShouldBeUpdated(UserAggregate updatedBy)
        {
            audit.UpdatedBy.Should().Be(updatedBy);
            audit.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }
    }

    extension(List<string> orderBy)
    {
        internal string ToOrderByQueryParam()
        {
            return string.Join('&', orderBy.Select(o => $"orderBy={o}"));
        }

        internal string ToSearchQueryParam()
        {
            return string.Join('&', orderBy.Select(s => $"search={s}"));
        }
    }
}
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Infrastructure.Extensions;
using ConsiliumTempus.Infrastructure.Security.Authorization.Http;
using ErrorOr;
using Microsoft.AspNetCore.Http;

namespace ConsiliumTempus.Infrastructure.Security.Authorization.Targets;

/// <summary>
/// The kind of entity a request points to, through the id it carries
/// </summary>
internal enum TargetKind
{
    CustomFieldSetup,
    Project,
    ProjectSprint,
    ProjectStage,
    ProjectTask,
    Workspace
}

internal enum IdLocation
{
    Route,
    Query,
    Body
}

/// <summary>
/// Where the id of the targeted entity is in a request, and what kind of entity it identifies.
/// When it is <paramref name="Qualified"/>, the id is read from the property named after the kind of entity
/// (e.g. <c>projectId</c>) instead of the default <c>id</c>.
/// </summary>
internal readonly record struct TargetId(TargetKind Kind, IdLocation Location, bool Qualified = false)
{
    internal string Property => Qualified ? Kind.ToString().FromPascalToCamelCase().ToId() : "id";

    internal Task<string?> ReadAsync(HttpRequest request)
    {
        return Location switch
        {
            IdLocation.Route => Task.FromResult(HttpRequestReader.GetStringIdFromRoute(request, Property)),
            IdLocation.Query => Task.FromResult(HttpRequestReader.GetStringIdFromQuery(request, Property)),
            IdLocation.Body => HttpRequestReader.GetStringIdFromBody(request, Property),
            _ => throw new ArgumentOutOfRangeException(nameof(Location), Location, null)
        };
    }
}

internal static class TargetKindExtensions
{
    internal static Error NotFoundError(this TargetKind kind)
    {
        return kind switch
        {
            TargetKind.CustomFieldSetup => Errors.CustomFieldSetup.NotFound,
            TargetKind.Project => Errors.Project.NotFound,
            TargetKind.ProjectSprint => Errors.ProjectSprint.NotFound,
            TargetKind.ProjectStage => Errors.ProjectStage.NotFound,
            TargetKind.ProjectTask => Errors.ProjectTask.NotFound,
            TargetKind.Workspace => Errors.Workspace.NotFound,
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
        };
    }
}

using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Project.GetCollection;

public sealed class GetCollectionProjectRequest
{
    [FromQuery] public int? PageSize { get; init; }

    [FromQuery] public int? CurrentPage { get; init; }

    [FromQuery] public string? Order { get; init; }

    [FromQuery] public Guid? WorkspaceId { get; init; }

    [FromQuery] public string? Name { get; init; }

    [FromQuery] public bool? IsFavorite { get; init; }

    [FromQuery] public bool? IsPrivate { get; init; }
}
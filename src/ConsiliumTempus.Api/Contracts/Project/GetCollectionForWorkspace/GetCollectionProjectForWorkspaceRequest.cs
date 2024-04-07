using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Project.GetCollectionForWorkspace;

public sealed class GetCollectionProjectForWorkspaceRequest
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
    [FromQuery]
    public Guid WorkspaceId { get; set; }
}
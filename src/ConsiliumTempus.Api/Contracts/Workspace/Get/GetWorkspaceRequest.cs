using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Workspace.Get;

public sealed class GetWorkspaceRequest
{
    [FromRoute]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public Guid Id { get; set; }
}
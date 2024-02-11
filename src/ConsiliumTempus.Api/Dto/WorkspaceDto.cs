using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Dto;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record WorkspaceDto(
    string Id,
    string Name,
    string Description);
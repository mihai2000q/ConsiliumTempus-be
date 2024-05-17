using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.ProjectTask.Get;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetProjectTaskResponse(
    string Name,
    string Description,
    bool IsCompleted);
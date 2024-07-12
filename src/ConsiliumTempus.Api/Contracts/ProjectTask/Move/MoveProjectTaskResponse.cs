using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.ProjectTask.Move;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record MoveProjectTaskResponse(string Message);
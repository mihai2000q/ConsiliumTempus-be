using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.ProjectTask.Create;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record CreateProjectTaskResponse(string Message);
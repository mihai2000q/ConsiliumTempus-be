using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Project.Create;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record CreateProjectResponse(string Message);
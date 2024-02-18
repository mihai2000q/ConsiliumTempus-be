using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Project.Delete;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record DeleteProjectResponse(string Message);
using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Project.Update;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record UpdateProjectResponse(string Message);
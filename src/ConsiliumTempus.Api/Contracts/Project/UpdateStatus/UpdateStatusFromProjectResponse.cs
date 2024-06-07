using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Project.UpdateStatus;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record UpdateStatusFromProjectResponse(string Message);
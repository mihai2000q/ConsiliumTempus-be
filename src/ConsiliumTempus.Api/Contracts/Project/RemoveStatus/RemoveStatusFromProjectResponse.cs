using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Project.RemoveStatus;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record RemoveStatusFromProjectResponse(string Message);
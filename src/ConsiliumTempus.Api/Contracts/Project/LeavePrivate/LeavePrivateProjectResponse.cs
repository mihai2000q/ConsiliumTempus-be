using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Project.LeavePrivate;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record LeavePrivateProjectResponse(string Message);
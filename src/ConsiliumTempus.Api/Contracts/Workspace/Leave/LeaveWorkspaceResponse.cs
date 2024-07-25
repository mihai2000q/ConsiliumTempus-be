using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Workspace.Leave;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record LeaveWorkspaceResponse(string Message);
using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Workspace.Delete;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record DeleteWorkspaceResponse(string Message);
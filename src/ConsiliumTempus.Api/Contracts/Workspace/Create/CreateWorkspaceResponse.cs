using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Workspace.Create;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record CreateWorkspaceResponse(string Message);
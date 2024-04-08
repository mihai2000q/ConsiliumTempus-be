using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Workspace.Update;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record UpdateWorkspaceResponse(string Message);
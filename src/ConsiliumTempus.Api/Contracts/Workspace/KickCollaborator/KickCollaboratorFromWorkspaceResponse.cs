using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Workspace.KickCollaborator;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record KickCollaboratorFromWorkspaceResponse(string Message);
using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Workspace.InviteCollaborator;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record InviteCollaboratorToWorkspaceResponse(string Message);
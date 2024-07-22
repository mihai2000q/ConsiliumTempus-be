using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Workspace.RejectInvitation;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record RejectInvitationToWorkspaceResponse(string Message);
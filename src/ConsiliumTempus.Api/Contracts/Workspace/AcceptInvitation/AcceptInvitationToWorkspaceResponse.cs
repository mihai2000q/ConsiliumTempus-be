using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Workspace.AcceptInvitation;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record AcceptInvitationToWorkspaceResponse(string Message);
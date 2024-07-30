namespace ConsiliumTempus.Api.Contracts.Project.AddAllowedMember;

public sealed record AddAllowedMemberToProjectRequest(
    Guid Id,
    Guid CollaboratorId);
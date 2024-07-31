namespace ConsiliumTempus.Api.Contracts.Project.UpdateOwner;

public sealed record UpdateOwnerProjectRequest(
    Guid Id,
    Guid OwnerId);
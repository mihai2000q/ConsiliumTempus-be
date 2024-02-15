namespace ConsiliumTempus.Api.Contracts.Project.Create;

public sealed record CreateProjectRequest(
    Guid WorkspaceId,
    string Name,
    string Description,
    bool IsPrivate);
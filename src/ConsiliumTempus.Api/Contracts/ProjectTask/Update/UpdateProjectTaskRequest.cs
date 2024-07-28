namespace ConsiliumTempus.Api.Contracts.ProjectTask.Update;

public sealed record UpdateProjectTaskRequest(
    Guid Id,
    string Name,
    Guid? AssigneeId);
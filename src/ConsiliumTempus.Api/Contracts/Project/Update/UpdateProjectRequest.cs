using ConsiliumTempus.Domain.Project.Enums;

namespace ConsiliumTempus.Api.Contracts.Project.Update;

public sealed record UpdateProjectRequest(
    Guid Id,
    string Name,
    ProjectLifecycle Lifecycle);
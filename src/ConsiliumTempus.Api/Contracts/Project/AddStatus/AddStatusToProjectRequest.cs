using ConsiliumTempus.Domain.Project.Enums;

namespace ConsiliumTempus.Api.Contracts.Project.AddStatus;

public sealed record AddStatusToProjectRequest(
    Guid Id,
    string Title,
    ProjectStatusType Status,
    string Description);
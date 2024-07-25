using ConsiliumTempus.Domain.ProjectSprint.Entities;

namespace ConsiliumTempus.Application.ProjectSprint.Queries.GetStages;

public sealed record GetStagesFromProjectSprintResult(List<ProjectStage> Stages);
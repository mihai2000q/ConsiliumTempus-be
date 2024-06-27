using ConsiliumTempus.Domain.ProjectSprint.Entities;

namespace ConsiliumTempus.Application.ProjectSprint.Queries.GetStages;

public record GetStagesFromProjectSprintResult(List<ProjectStage> Stages);
using ConsiliumTempus.Domain.Project.Entities;

namespace ConsiliumTempus.Application.Project.Entities.Stage.Queries.GetCollection;

public sealed record GetCollectionProjectStageResult(
    List<ProjectStage> Stages);
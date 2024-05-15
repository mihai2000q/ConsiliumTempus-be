using ConsiliumTempus.Application.Project.Entities.Stage.Queries.GetCollection;

namespace ConsiliumTempus.Common.UnitTests.Project.Entities.Stage;

public static class ProjectStageQueryFactory
{
    public static GetCollectionProjectStageQuery CreateGetCollectionProjectStageQuery(Guid? projectSprintId = null)
    {
        return new GetCollectionProjectStageQuery(projectSprintId ?? Guid.NewGuid());
    }
}
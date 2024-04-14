using ConsiliumTempus.Application.Project.Entities.Sprint.Queries.GetCollection;

namespace ConsiliumTempus.Common.UnitTests.Project.Entities.ProjectSprint;

public static class ProjectSprintQueryFactory
{
    public static GetCollectionProjectSprintQuery CreateGetCollectionProjectSprintQuery(
        Guid? projectId = null)
    {
        return new GetCollectionProjectSprintQuery(
            projectId ?? Guid.NewGuid());
    }
}
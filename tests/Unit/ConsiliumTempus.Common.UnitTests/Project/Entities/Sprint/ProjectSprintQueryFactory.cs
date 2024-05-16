using ConsiliumTempus.Application.ProjectSprint.Queries.Get;
using ConsiliumTempus.Application.ProjectSprint.Queries.GetCollection;

namespace ConsiliumTempus.Common.UnitTests.Project.Entities.Sprint;

public static class ProjectSprintQueryFactory
{
    public static GetProjectSprintQuery CreateGetProjectSprintQuery(Guid? id = null)
    {
        return new GetProjectSprintQuery(id ?? Guid.NewGuid());
    }
    
    public static GetCollectionProjectSprintQuery CreateGetCollectionProjectSprintQuery(
        Guid? projectId = null)
    {
        return new GetCollectionProjectSprintQuery(
            projectId ?? Guid.NewGuid());
    }
}
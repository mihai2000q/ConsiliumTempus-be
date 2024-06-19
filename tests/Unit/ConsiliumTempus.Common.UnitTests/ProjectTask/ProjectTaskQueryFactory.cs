using ConsiliumTempus.Application.ProjectTask.Queries.Get;
using ConsiliumTempus.Application.ProjectTask.Queries.GetCollection;

namespace ConsiliumTempus.Common.UnitTests.ProjectTask;

public static class ProjectTaskQueryFactory
{
    public static GetProjectTaskQuery CreateGetProjectTaskQuery(Guid? id = null)
    {
        return new GetProjectTaskQuery(id ?? Guid.NewGuid());
    }
    
    public static GetCollectionProjectTaskQuery CreateGetCollectionProjectTaskQuery(
        Guid? projectStageId = null,
        string[]? search = null,
        string[]? orderBy = null)
    {
        return new GetCollectionProjectTaskQuery(
            projectStageId ?? Guid.NewGuid(),
            search,
            orderBy);
    }
}
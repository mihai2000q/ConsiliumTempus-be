using ConsiliumTempus.Application.ProjectTask.Queries.Get;

namespace ConsiliumTempus.Common.UnitTests.ProjectTask;

public static class ProjectTaskQueryFactory
{
    public static GetProjectTaskQuery CreateGetProjectTaskQuery(Guid? id = null)
    {
        return new GetProjectTaskQuery(id ?? Guid.NewGuid());
    }
}
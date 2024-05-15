using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Application.Project.Queries.GetCollection;
using ConsiliumTempus.Domain.Project;

namespace ConsiliumTempus.Common.UnitTests.Project;

public static class ProjectResultFactory
{
    public static GetCollectionProjectResult CreateGetCollectionProjectResult(
        List<ProjectAggregate>? projects = null,
        int totalCount = 25,
        int? totalPages = null)
    {
        return new GetCollectionProjectResult(
            projects ?? ProjectFactory.CreateList(),
            totalCount,
            totalPages);
    }
    
    public static CreateProjectResult CreateCreateProjectResult()
    {
        return new CreateProjectResult();
    }
    
    public static DeleteProjectResult CreateDeleteProjectResult()
    {
        return new DeleteProjectResult();
    }
}
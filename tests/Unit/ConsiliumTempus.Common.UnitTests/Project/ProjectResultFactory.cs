using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Application.Project.Commands.Update;
using ConsiliumTempus.Application.Project.Commands.UpdateOverview;
using ConsiliumTempus.Application.Project.Queries.GetCollection;
using ConsiliumTempus.Domain.Project;

namespace ConsiliumTempus.Common.UnitTests.Project;

public static class ProjectResultFactory
{
    public static GetCollectionProjectResult CreateGetCollectionProjectResult(
        List<ProjectAggregate>? projects = null,
        int totalCount = 25)
    {
        return new GetCollectionProjectResult(
            projects ?? ProjectFactory.CreateList(),
            totalCount);
    }

    public static CreateProjectResult CreateCreateProjectResult()
    {
        return new CreateProjectResult();
    }

    public static UpdateProjectResult CreateUpdateProjectResult()
    {
        return new UpdateProjectResult();
    }

    public static UpdateOverviewProjectResult CreateUpdateOverviewProjectResult()
    {
        return new UpdateOverviewProjectResult();
    }

    public static DeleteProjectResult CreateDeleteProjectResult()
    {
        return new DeleteProjectResult();
    }
}
using ConsiliumTempus.Application.Project.Queries.Get;
using ConsiliumTempus.Application.Project.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.User;

namespace ConsiliumTempus.Api.UnitTests.TestData;

internal static class ProjectControllerData
{
    internal class GetProjectResults : TheoryData<GetProjectResult>
    {
        public GetProjectResults()
        {
            Add(ProjectResultFactory.CreateGetProjectResult());
            Add(ProjectResultFactory.CreateGetProjectResult(ProjectFactory.CreateWithStatuses()));
        }
    }

    internal class GetCollectionProjectResults : TheoryData<GetCollectionProjectResult>
    {
        public GetCollectionProjectResults()
        {
            Add(ProjectResultFactory.CreateGetCollectionProjectResult());
            Add(ProjectResultFactory.CreateGetCollectionProjectResult(
                [
                    ProjectFactory.Create(),
                    ProjectFactory.CreateWithStatuses(),
                    ProjectFactory.CreateWithStatuses(statusesCount: 1)
                ]));
        }
    }
}

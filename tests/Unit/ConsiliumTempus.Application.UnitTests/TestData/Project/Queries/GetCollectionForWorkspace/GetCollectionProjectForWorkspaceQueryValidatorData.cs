using ConsiliumTempus.Application.Project.Queries.GetCollectionForWorkspace;
using ConsiliumTempus.Common.UnitTests.Project;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Queries.GetCollectionForWorkspace;

internal static class GetCollectionProjectForWorkspaceQueryValidatorData
{
    internal class GetValidQueries : TheoryData<GetCollectionProjectForWorkspaceQuery>
    {
        public GetValidQueries()
        {
            var query = ProjectQueryFactory.CreateGetCollectionProjectForWorkspaceQuery();
            Add(query);
        }
    }
    
    internal class GetInvalidWorkspaceIdQueries : TheoryData<GetCollectionProjectForWorkspaceQuery, string>
    {
        public GetInvalidWorkspaceIdQueries()
        {
            var query = ProjectQueryFactory.CreateGetCollectionProjectForWorkspaceQuery(workspaceId: Guid.Empty);
            Add(query, nameof(query.WorkspaceId));
        }
    }
}
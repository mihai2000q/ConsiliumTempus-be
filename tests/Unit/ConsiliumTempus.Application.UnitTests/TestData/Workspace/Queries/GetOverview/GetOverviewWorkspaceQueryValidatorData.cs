using ConsiliumTempus.Application.Workspace.Queries.GetOverview;
using ConsiliumTempus.Common.UnitTests.Workspace;

namespace ConsiliumTempus.Application.UnitTests.TestData.Workspace.Queries.GetOverview;

internal static class GetOverviewWorkspaceQueryValidatorData
{
    internal class GetOverviewValidQueries : TheoryData<GetOverviewWorkspaceQuery>
    {
        public GetOverviewValidQueries()
        {
            var query = WorkspaceQueryFactory.CreateGetOverviewWorkspaceQuery();
            Add(query);
            
            query = new GetOverviewWorkspaceQuery(Guid.NewGuid());
            Add(query);
        }
    }
    
    internal class GetOverviewInvalidIdQueries : TheoryData<GetOverviewWorkspaceQuery, string>
    {
        public GetOverviewInvalidIdQueries()
        {
            var query = WorkspaceQueryFactory.CreateGetOverviewWorkspaceQuery(id: Guid.Empty);
            Add(query, nameof(query.Id));
        }
    }
}
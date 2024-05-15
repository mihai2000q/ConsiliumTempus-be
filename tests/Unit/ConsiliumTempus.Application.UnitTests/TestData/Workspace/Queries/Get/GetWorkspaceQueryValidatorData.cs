using ConsiliumTempus.Application.Workspace.Queries.Get;
using ConsiliumTempus.Common.UnitTests.Workspace;

namespace ConsiliumTempus.Application.UnitTests.TestData.Workspace.Queries.Get;

internal static class GetWorkspaceQueryValidatorData
{
    internal class GetValidQueries : TheoryData<GetWorkspaceQuery>
    {
        public GetValidQueries()
        {
            var query = WorkspaceQueryFactory.CreateGetWorkspaceQuery();
            Add(query);
            
            query = new GetWorkspaceQuery(Guid.NewGuid());
            Add(query);
        }
    }
    
    internal class GetInvalidIdQueries : TheoryData<GetWorkspaceQuery, string>
    {
        public GetInvalidIdQueries()
        {
            var query = WorkspaceQueryFactory.CreateGetWorkspaceQuery(id: Guid.Empty);
            Add(query, nameof(query.Id));
        }
    }
}
using ConsiliumTempus.Application.Workspace.Queries.GetCollaborators;
using ConsiliumTempus.Common.UnitTests.Workspace;

namespace ConsiliumTempus.Application.UnitTests.TestData.Workspace.Queries.GetCollaborators;

public static class GetCollaboratorsFromWorkspaceQueryValidatorData
{
    internal class GetValidQueries : TheoryData<GetCollaboratorsFromWorkspaceQuery>
    {
        public GetValidQueries()
        {
            var query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery();
            Add(query);
            
            query = new GetCollaboratorsFromWorkspaceQuery(
                Guid.NewGuid(),
                "Michael");
            Add(query);
        }
    }
    
    internal class GetInvalidIdQueries : TheoryData<GetCollaboratorsFromWorkspaceQuery, string>
    {
        public GetInvalidIdQueries()
        {
            var query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(id: Guid.Empty);
            Add(query, nameof(query.Id));
        }
    }
}
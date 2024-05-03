using ConsiliumTempus.Application.Workspace.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Validation;

namespace ConsiliumTempus.Application.UnitTests.TestData.Workspace.Queries.GetCollection;

internal static class GetCollectionWorkspaceQueryValidatorData
{
    internal class GetValidQueries : TheoryData<GetCollectionWorkspaceQuery>
    {
        public GetValidQueries()
        {
            var query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery();
            Add(query);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                order: "name.asc");
            Add(query);
        }
    }
    
    internal class GetInvalidOrderQueries : TheoryData<GetCollectionWorkspaceQuery, string, int>
    {
        public GetInvalidOrderQueries()
        {
            var query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                order: "");
            Add(query, nameof(query.Order), 1);
            
            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                order: "something");
            Add(query, nameof(query.Order), 1);
            
            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                order: "something.");
            Add(query, nameof(query.Order), 2);
            
            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                order: "something.descending");
            Add(query, nameof(query.Order), 2);
            
            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                order: "LastActivity.desc");
            Add(query, nameof(query.Order), 1);
            
            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                order: "not_a_property.asc");
            Add(query, nameof(query.Order), 1);
        }
    }
    
    internal class GetInvalidNameQueries : TheoryData<GetCollectionWorkspaceQuery, string, int>
    {
        public GetInvalidNameQueries()
        {
            var query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                name: new string('*', PropertiesValidation.Workspace.NameMaximumLength + 1));
            Add(query, nameof(query.Name), 1);
        }
    }
}
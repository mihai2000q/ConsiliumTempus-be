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
                orders: ["name.asc", "last_activity.asc", "created_date_time.asc", "updated_date_time.asc"]);
            Add(query);
            
            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                orders: ["name.desc", "last_activity.desc", "created_date_time.desc","updated_date_time.desc"]);
            Add(query);

            query = new GetCollectionWorkspaceQuery(
                true,
                10,
                2,
                ["name.asc" , "last_activity.desc"],
                "proj");
            Add(query);
        }
    }

    internal class GetInvalidOrdersQueries : TheoryData<GetCollectionWorkspaceQuery, string, int>
    {
        public GetInvalidOrdersQueries()
        {
            var query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                orders: [""]);
            Add(query, nameof(query.Orders), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                orders: ["something"]);
            Add(query, nameof(query.Orders), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                orders: ["something", "another thing", "something else"]);
            Add(query, nameof(query.Orders), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                orders: ["something."]);
            Add(query, nameof(query.Orders), 2);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                orders: ["something.descending"]);
            Add(query, nameof(query.Orders), 2);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                orders: ["name.asc", "last_activity.descending"]);
            Add(query, nameof(query.Orders), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                orders: ["LastActivity.desc"]);
            Add(query, nameof(query.Orders), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                orders: ["last_activity.desc", "Name.asc"]);
            Add(query, nameof(query.Orders), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                orders: ["not_a_property.asc"]);
            Add(query, nameof(query.Orders), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                orders: ["name.asc", "not_a_property.asc"]);
            Add(query, nameof(query.Orders), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                orders: ["name.asc", "name.desc"]);
            Add(query, nameof(query.Orders), 1);
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

    internal class GetInvalidPageSizeQueries : TheoryData<GetCollectionWorkspaceQuery, string, int>
    {
        public GetInvalidPageSizeQueries()
        {
            var query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                pageSize: 0);
            Add(query, nameof(query.PageSize), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                pageSize: -1);
            Add(query, nameof(query.PageSize), 1);
        }
    }

    internal class GetInvalidCurrentPageQueries : TheoryData<GetCollectionWorkspaceQuery, string, int>
    {
        public GetInvalidCurrentPageQueries()
        {
            var query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                currentPage: 0);
            Add(query, nameof(query.CurrentPage), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                currentPage: -1);
            Add(query, nameof(query.CurrentPage), 1);
        }
    }
}
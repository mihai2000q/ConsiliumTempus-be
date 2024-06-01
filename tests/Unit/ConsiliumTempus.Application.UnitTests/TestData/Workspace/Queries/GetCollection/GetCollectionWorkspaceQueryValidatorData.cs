using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Workspace.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.Workspace;

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
                orderBy: [],
                search: []);
            Add(query);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                orderBy: ["name.asc", "last_activity.asc", "created_date_time.asc", "updated_date_time.asc"]);
            Add(query);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                orderBy: ["name.desc", "last_activity.desc", "created_date_time.desc", "updated_date_time.desc"]);
            Add(query);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                search:
                [
                    "name ct something", "name sw something", "name eq something", "name neq something",
                ]);
            Add(query);

            query = new GetCollectionWorkspaceQuery(
                true,
                10,
                2,
                ["name.asc", "last_activity.desc"],
                ["name ct proj"]);
            Add(query);
        }
    }

    internal class GetInvalidPageSizeAndCurrentPageQueries : TheoryData<GetCollectionWorkspaceQuery, string, short>
    {
        public GetInvalidPageSizeAndCurrentPageQueries()
        {
            var query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                pageSize: 1);
            Add(query, nameof(query.PageSize).And(nameof(query.CurrentPage)), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                currentPage: 1);
            Add(query, nameof(query.PageSize).And(nameof(query.CurrentPage)), 1);
        }
    }

    internal class GetInvalidPageSizeQueries : TheoryData<GetCollectionWorkspaceQuery, string, int>
    {
        public GetInvalidPageSizeQueries()
        {
            var query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                currentPage: 1,
                pageSize: 0);
            Add(query, nameof(query.PageSize), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                currentPage: 1,
                pageSize: -1);
            Add(query, nameof(query.PageSize), 1);
        }
    }

    internal class GetInvalidCurrentPageQueries : TheoryData<GetCollectionWorkspaceQuery, string, int>
    {
        public GetInvalidCurrentPageQueries()
        {
            var query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                pageSize: 20,
                currentPage: 0);
            Add(query, nameof(query.CurrentPage), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                pageSize: 20,
                currentPage: -1);
            Add(query, nameof(query.CurrentPage), 1);
        }
    }

    internal class GetInvalidOrderByQueries : TheoryData<GetCollectionWorkspaceQuery, string, int>
    {
        public GetInvalidOrderByQueries()
        {
            const string correct = "name.asc";

            // Separator Validation
            var query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                orderBy: [""]);
            Add(query, nameof(query.OrderBy), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                orderBy: ["something"]);
            Add(query, nameof(query.OrderBy), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                orderBy: [correct, "something", "another thing", "something else"]);
            Add(query, nameof(query.OrderBy), 1);

            // Order Type Validation
            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                orderBy: ["name."]);
            Add(query, nameof(query.OrderBy), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                orderBy: ["name.descending"]);
            Add(query, nameof(query.OrderBy), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                orderBy: [correct, "last_activity.descending"]);
            Add(query, nameof(query.OrderBy), 1);

            // Snake Case Validation
            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                orderBy: ["LastActivity.desc"]);
            Add(query, nameof(query.OrderBy), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                orderBy: [correct, "Name.asc"]);
            Add(query, nameof(query.OrderBy), 1);

            // Property Validation
            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                orderBy: ["not_a_property.asc"]);
            Add(query, nameof(query.OrderBy), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                orderBy: [correct, "not_a_property.asc"]);
            Add(query, nameof(query.OrderBy), 1);

            // Repetition Validation
            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                orderBy: ["name.asc", "name.desc"]);
            Add(query, nameof(query.OrderBy), 1);
        }
    }

    internal class GetInvalidSearchQueries : TheoryData<GetCollectionWorkspaceQuery, string, short>
    {
        public GetInvalidSearchQueries()
        {
            const string correct = "name eq something";

            // Separator Validation
            var query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                search: [""]);
            Add(query, nameof(query.Search), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                search: ["name"]);
            Add(query, nameof(query.Search), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                search: ["name eq"]);
            Add(query, nameof(query.Search), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                search: ["name eq "]);
            Add(query, nameof(query.Search), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                search: ["name eq     "]);
            Add(query, nameof(query.Search), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                search: ["name eq     "]);
            Add(query, nameof(query.Search), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                search: [correct, "name eq     "]);
            Add(query, nameof(query.Search), 1);

            // Snake Case Validation
            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                search: ["Name eq something"]);
            Add(query, nameof(query.Search), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                search: [correct, "Name eq something"]);
            Add(query, nameof(query.Search), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                search: ["NotProperty eq something"]);
            Add(query, nameof(query.Search), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                search: [correct, "NotProperty eq something"]);
            Add(query, nameof(query.Search), 1);

            // Property Validation
            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                search: ["not_a_property eq something"]);
            Add(query, nameof(query.Search), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                search: [correct, "not_a_property eq something"]);
            Add(query, nameof(query.Search), 1);

            // Operator Validation
            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                search: ["name smth something"]);
            Add(query, nameof(query.Search), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                search: [correct, "name smth something"]);
            Add(query, nameof(query.Search), 1);

            // Operator And Type Validation
            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                search: ["name lte something"]);
            Add(query, nameof(query.Search), 1);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                search: [correct, "name lte something"]);
            Add(query, nameof(query.Search), 1);
        }
    }
}
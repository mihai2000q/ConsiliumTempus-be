using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Project.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.Project;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Queries.GetCollection;

internal static class GetCollectionProjectQueryValidatorData
{
    internal class GetValidQueries : TheoryData<GetCollectionProjectQuery>
    {
        public GetValidQueries()
        {
            var query = ProjectQueryFactory.CreateGetCollectionProjectQuery();
            Add(query);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                orderBy: [],
                search: []);
            Add(query);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                orderBy: ["name.asc", "last_activity.asc", "created_date_time.asc", "updated_date_time.asc"]);
            Add(query);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                orderBy: ["name.desc", "last_activity.desc", "created_date_time.desc", "updated_date_time.desc"]);
            Add(query);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                search:
                [
                    "name ct something", "name sw something", "name eq something", "name neq something",
                    "is_favorite eq true", "is_favorite neq false",
                    "is_private eq true", "is_private neq false",
                    "lifecycle eq active", "lifecycle neq archived", "lifecycle eq uPcOmING"
                ]);
            Add(query);

            query = new GetCollectionProjectQuery(
                10,
                2,
                ["name.desc"],
                ["name sw New Project"],
                Guid.NewGuid());
            Add(query);
        }
    }

    internal class GetInvalidPageSizeAndCurrentPageQueries : TheoryData<GetCollectionProjectQuery, string, short>
    {
        public GetInvalidPageSizeAndCurrentPageQueries()
        {
            var query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                pageSize: 1);
            Add(query, nameof(query.PageSize).And(nameof(query.CurrentPage)), 1);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                currentPage: 1);
            Add(query, nameof(query.PageSize).And(nameof(query.CurrentPage)), 1);
        }
    }

    internal class GetInvalidPageSizeQueries : TheoryData<GetCollectionProjectQuery, string, short>
    {
        public GetInvalidPageSizeQueries()
        {
            var query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                currentPage: 1,
                pageSize: -1);
            Add(query, nameof(query.PageSize), 1);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                currentPage: 1,
                pageSize: 0);
            Add(query, nameof(query.PageSize), 1);
        }
    }

    internal class GetInvalidCurrentPageQueries : TheoryData<GetCollectionProjectQuery, string, short>
    {
        public GetInvalidCurrentPageQueries()
        {
            var query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                pageSize: 20,
                currentPage: -1);
            Add(query, nameof(query.CurrentPage), 1);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                pageSize: 20,
                currentPage: 0);
            Add(query, nameof(query.CurrentPage), 1);
        }
    }

    internal class GetInvalidOrderByQueries : TheoryData<GetCollectionProjectQuery, string, short>
    {
        public GetInvalidOrderByQueries()
        {
            const string correct = "name.asc";

            // Separator Validation
            var query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                orderBy: [""]);
            Add(query, nameof(query.OrderBy), 1);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                orderBy: ["something"]);
            Add(query, nameof(query.OrderBy), 1);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                orderBy: [correct, "something", "another"]);
            Add(query, nameof(query.OrderBy), 1);

            // Order Type Validation
            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                orderBy: ["name."]);
            Add(query, nameof(query.OrderBy), 1);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                orderBy: ["name.descending"]);
            Add(query, nameof(query.OrderBy), 1);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                orderBy: [correct, "last_activity.descending"]);
            Add(query, nameof(query.OrderBy), 1);

            // Snake Case Validation
            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                orderBy: ["LastActivity.desc"]);
            Add(query, nameof(query.OrderBy), 1);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                orderBy: [correct, "LastActivity.desc"]);
            Add(query, nameof(query.OrderBy), 1);

            // Property Validation
            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                orderBy: ["not_a_property.asc"]);
            Add(query, nameof(query.OrderBy), 1);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                orderBy: [correct, "not_property.desc"]);
            Add(query, nameof(query.OrderBy), 1);

            // Repetition Validation
            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                orderBy: ["name.asc", "name.desc"]);
            Add(query, nameof(query.OrderBy), 1);
        }
    }

    internal class GetInvalidSearchQueries : TheoryData<GetCollectionProjectQuery, string, short>
    {
        public GetInvalidSearchQueries()
        {
            const string correct = "name eq something";

            // Separator Validation
            var query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                search: [""]);
            Add(query, nameof(query.Search), 1);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                search: ["name"]);
            Add(query, nameof(query.Search), 1);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                search: ["name eq"]);
            Add(query, nameof(query.Search), 1);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                search: ["name eq "]);
            Add(query, nameof(query.Search), 1);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                search: ["name eq     "]);
            Add(query, nameof(query.Search), 1);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                search: ["name eq     "]);
            Add(query, nameof(query.Search), 1);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                search: [correct, "name eq     "]);
            Add(query, nameof(query.Search), 1);

            // Snake Case Validation
            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                search: ["Name eq something"]);
            Add(query, nameof(query.Search), 1);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                search: [correct, "Name eq something"]);
            Add(query, nameof(query.Search), 1);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                search: ["NotProperty eq something"]);
            Add(query, nameof(query.Search), 1);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                search: [correct, "NotProperty eq something"]);
            Add(query, nameof(query.Search), 1);

            // Property Validation
            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                search: ["not_a_property eq something"]);
            Add(query, nameof(query.Search), 1);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                search: [correct, "not_a_property eq something"]);
            Add(query, nameof(query.Search), 1);

            // Operator Validation
            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                search: ["name smth something"]);
            Add(query, nameof(query.Search), 1);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                search: [correct, "name smth something"]);
            Add(query, nameof(query.Search), 1);

            // Operator And Type Validation
            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                search: ["name lte something"]);
            Add(query, nameof(query.Search), 1);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                search: [correct, "name lte something"]);
            Add(query, nameof(query.Search), 1);
            
            // Parse Validation
            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                search: ["is_favorite eq something"]);
            Add(query, nameof(query.Search), 1);
            
            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                search: ["lifecycle eq notACycle"]);
            Add(query, nameof(query.Search), 1);
            
            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                search: [correct, "is_favorite eq something"]);
            Add(query, nameof(query.Search), 1);
        }
    }
}
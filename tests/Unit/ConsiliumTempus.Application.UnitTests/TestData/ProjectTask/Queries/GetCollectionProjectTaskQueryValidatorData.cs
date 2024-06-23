using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.ProjectTask.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.ProjectTask;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Queries;

internal static class GetCollectionProjectTaskQueryValidatorData
{
    internal class GetValidQueries : TheoryData<GetCollectionProjectTaskQuery>
    {
        public GetValidQueries()
        {
            var query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery();
            Add(query);

            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                search:
                [
                    // Name
                    "name ct something", "name sw something", "name eq something", "name neq something",
                    // Is Completed
                    "is_completed eq true", "is_completed neq false"
                ]);
            Add(query);

            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                orderBy:
                [
                    "name.asc", "is_completed.asc", "created_date_time.asc", "updated_date_time.asc"
                ]);
            Add(query);

            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                orderBy:
                [
                    "name.desc", "is_completed.desc", "created_date_time.desc", "updated_date_time.desc"
                ]);
            Add(query);

            query = new GetCollectionProjectTaskQuery(
                Guid.NewGuid(),
                ["name ct Screen"],
                ["name.asc"],
                1,
                20);
            Add(query);
        }
    }

    internal class GetInvalidProjectStageIdQueries : TheoryData<GetCollectionProjectTaskQuery, string, short>
    {
        public GetInvalidProjectStageIdQueries()
        {
            var query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(Guid.Empty);
            Add(query, nameof(query.ProjectStageId), 1);
        }
    }
    
    internal class GetInvalidPageSizeAndCurrentPageQueries : TheoryData<GetCollectionProjectTaskQuery, string, short>
    {
        public GetInvalidPageSizeAndCurrentPageQueries()
        {
            var query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                pageSize: 1);
            Add(query, nameof(query.PageSize).And(nameof(query.CurrentPage)), 1);

            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                currentPage: 1);
            Add(query, nameof(query.PageSize).And(nameof(query.CurrentPage)), 1);
        }
    }

    internal class GetInvalidPageSizeQueries : TheoryData<GetCollectionProjectTaskQuery, string, short>
    {
        public GetInvalidPageSizeQueries()
        {
            var query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                currentPage: 1,
                pageSize: -1);
            Add(query, nameof(query.PageSize), 1);

            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                currentPage: 1,
                pageSize: 0);
            Add(query, nameof(query.PageSize), 1);
        }
    }

    internal class GetInvalidCurrentPageQueries : TheoryData<GetCollectionProjectTaskQuery, string, short>
    {
        public GetInvalidCurrentPageQueries()
        {
            var query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                pageSize: 20,
                currentPage: -1);
            Add(query, nameof(query.CurrentPage), 1);

            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                pageSize: 20,
                currentPage: 0);
            Add(query, nameof(query.CurrentPage), 1);
        }
    }

    internal class GetInvalidOrderByQueries : TheoryData<GetCollectionProjectTaskQuery, string, short>
    {
        public GetInvalidOrderByQueries()
        {
            const string correct = "name.asc";

            // Separator Validation
            var query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                orderBy: [""]);
            Add(query, nameof(query.OrderBy), 1);

            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                orderBy: ["something"]);
            Add(query, nameof(query.OrderBy), 1);

            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                orderBy: [correct, "something", "another"]);
            Add(query, nameof(query.OrderBy), 1);

            // Order Type Validation
            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                orderBy: ["name."]);
            Add(query, nameof(query.OrderBy), 1);

            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                orderBy: ["name.descending"]);
            Add(query, nameof(query.OrderBy), 1);

            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                orderBy: [correct, "is_completed.descending"]);
            Add(query, nameof(query.OrderBy), 1);

            // Snake Case Validation
            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                orderBy: ["IsCompleted.desc"]);
            Add(query, nameof(query.OrderBy), 1);

            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                orderBy: [correct, "IsCompleted.desc"]);
            Add(query, nameof(query.OrderBy), 1);

            // Property Validation
            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                orderBy: ["not_a_property.asc"]);
            Add(query, nameof(query.OrderBy), 1);

            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                orderBy: [correct, "not_property.desc"]);
            Add(query, nameof(query.OrderBy), 1);

            // Repetition Validation
            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                orderBy: ["name.asc", "name.desc"]);
            Add(query, nameof(query.OrderBy), 1);
        }
    }

    internal class GetInvalidSearchQueries : TheoryData<GetCollectionProjectTaskQuery, string, short>
    {
        public GetInvalidSearchQueries()
        {
            const string correct = "name eq something";

            // Separator Validation
            var query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                search: [""]);
            Add(query, nameof(query.Search), 1);

            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                search: ["name"]);
            Add(query, nameof(query.Search), 1);

            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                search: ["name eq"]);
            Add(query, nameof(query.Search), 1);

            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                search: ["name eq "]);
            Add(query, nameof(query.Search), 1);

            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                search: ["name eq     "]);
            Add(query, nameof(query.Search), 1);

            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                search: [correct, "name eq     "]);
            Add(query, nameof(query.Search), 1);

            // Snake Case Validation
            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                search: ["Name eq something"]);
            Add(query, nameof(query.Search), 1);

            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                search: [correct, "Name eq something"]);
            Add(query, nameof(query.Search), 1);

            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                search: ["NotProperty eq something"]);
            Add(query, nameof(query.Search), 1);

            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                search: [correct, "NotProperty eq something"]);
            Add(query, nameof(query.Search), 1);

            // Property Validation
            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                search: ["not_a_property eq something"]);
            Add(query, nameof(query.Search), 1);

            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                search: [correct, "not_a_property eq something"]);
            Add(query, nameof(query.Search), 1);

            // Operator Validation
            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                search: ["name smth something"]);
            Add(query, nameof(query.Search), 1);

            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                search: [correct, "name smth something"]);
            Add(query, nameof(query.Search), 1);

            // Operator And Type Validation
            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                search: ["name lte something"]);
            Add(query, nameof(query.Search), 1);

            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                search: [correct, "name lte offtrack"]);
            Add(query, nameof(query.Search), 1);

            // Parse Validation
            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                search: ["is_completed eq something"]);
            Add(query, nameof(query.Search), 1);

            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                search: [correct, "is_completed eq something"]);
            Add(query, nameof(query.Search), 1);
        }
    }
}
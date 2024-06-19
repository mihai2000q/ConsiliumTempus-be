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

            query = new GetCollectionProjectTaskQuery(
                Guid.NewGuid(),
                ["name ct Screen"]);
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
using ConsiliumTempus.Application.ProjectSprint.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectSprint.Queries;

internal static class GetCollectionProjectSprintQueryValidatorData
{
    internal class GetValidQueries : TheoryData<GetCollectionProjectSprintQuery>
    {
        public GetValidQueries()
        {
            var query = ProjectSprintQueryFactory.CreateGetCollectionProjectSprintQuery();
            Add(query);
            
            query = ProjectSprintQueryFactory.CreateGetCollectionProjectSprintQuery(
                search: 
                [
                    // Name
                    "name ct something", "name sw something", "name eq something", "name neq something"
                ]);
            Add(query);

            query = new GetCollectionProjectSprintQuery(
                Guid.NewGuid(),
                ["name ct new sprint"],
                true);
            Add(query);
        }
    } 
    
    internal class GetInvalidProjectIdQueries : TheoryData<GetCollectionProjectSprintQuery, string, short>
    {
        public GetInvalidProjectIdQueries()
        {
            var query = ProjectSprintQueryFactory.CreateGetCollectionProjectSprintQuery(
                projectId: Guid.Empty);
            Add(query, nameof(query.ProjectId), 1);
        }
    } 
    
    internal class GetInvalidSearchQueries : TheoryData<GetCollectionProjectSprintQuery, string, short>
    {
        public GetInvalidSearchQueries()
        {
            const string correct = "name eq something";

            // Separator Validation
            var query = ProjectSprintQueryFactory.CreateGetCollectionProjectSprintQuery(
                search: [""]);
            Add(query, nameof(query.Search), 1);

            query = ProjectSprintQueryFactory.CreateGetCollectionProjectSprintQuery(
                search: ["name"]);
            Add(query, nameof(query.Search), 1);

            query = ProjectSprintQueryFactory.CreateGetCollectionProjectSprintQuery(
                search: ["name eq"]);
            Add(query, nameof(query.Search), 1);

            query = ProjectSprintQueryFactory.CreateGetCollectionProjectSprintQuery(
                search: ["name eq "]);
            Add(query, nameof(query.Search), 1);

            query = ProjectSprintQueryFactory.CreateGetCollectionProjectSprintQuery(
                search: ["name eq     "]);
            Add(query, nameof(query.Search), 1);

            query = ProjectSprintQueryFactory.CreateGetCollectionProjectSprintQuery(
                search: [correct, "name eq     "]);
            Add(query, nameof(query.Search), 1);

            // Snake Case Validation
            query = ProjectSprintQueryFactory.CreateGetCollectionProjectSprintQuery(
                search: ["Name eq something"]);
            Add(query, nameof(query.Search), 1);

            query = ProjectSprintQueryFactory.CreateGetCollectionProjectSprintQuery(
                search: [correct, "Name eq something"]);
            Add(query, nameof(query.Search), 1);

            query = ProjectSprintQueryFactory.CreateGetCollectionProjectSprintQuery(
                search: ["NotProperty eq something"]);
            Add(query, nameof(query.Search), 1);

            query = ProjectSprintQueryFactory.CreateGetCollectionProjectSprintQuery(
                search: [correct, "NotProperty eq something"]);
            Add(query, nameof(query.Search), 1);

            // Property Validation
            query = ProjectSprintQueryFactory.CreateGetCollectionProjectSprintQuery(
                search: ["not_a_property eq something"]);
            Add(query, nameof(query.Search), 1);

            query = ProjectSprintQueryFactory.CreateGetCollectionProjectSprintQuery(
                search: [correct, "not_a_property eq something"]);
            Add(query, nameof(query.Search), 1);

            // Operator Validation
            query = ProjectSprintQueryFactory.CreateGetCollectionProjectSprintQuery(
                search: ["name smth something"]);
            Add(query, nameof(query.Search), 1);

            query = ProjectSprintQueryFactory.CreateGetCollectionProjectSprintQuery(
                search: [correct, "name smth something"]);
            Add(query, nameof(query.Search), 1);

            // Operator And Type Validation
            query = ProjectSprintQueryFactory.CreateGetCollectionProjectSprintQuery(
                search: ["name lte something"]);
            Add(query, nameof(query.Search), 1);
        }
    } 
}
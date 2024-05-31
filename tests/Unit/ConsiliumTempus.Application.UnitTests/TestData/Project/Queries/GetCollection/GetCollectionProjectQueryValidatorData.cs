using ConsiliumTempus.Application.Project.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Domain.Common.Validation;

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
                orders: ["name.asc", "last_activity.asc", "created_date_time.asc", "updated_date_time.asc"]);
            Add(query);
            
            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                orders: ["name.desc", "last_activity.desc", "created_date_time.desc", "updated_date_time.desc"]);
            Add(query);

            query = new GetCollectionProjectQuery(
                10,
                2,
                ["name.desc"],
                Guid.NewGuid(),
                "New Project",
                false,
                true);
            Add(query);
        }
    }

    internal class GetInvalidPageSizeQueries : TheoryData<GetCollectionProjectQuery, string, short>
    {
        public GetInvalidPageSizeQueries()
        {
            var query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                pageSize: -1);
            Add(query, nameof(query.PageSize), 1);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                pageSize: 0);
            Add(query, nameof(query.PageSize), 1);
        }
    }

    internal class GetInvalidCurrentPageQueries : TheoryData<GetCollectionProjectQuery, string, short>
    {
        public GetInvalidCurrentPageQueries()
        {
            var query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                currentPage: -1);
            Add(query, nameof(query.CurrentPage), 1);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                currentPage: 0);
            Add(query, nameof(query.CurrentPage), 1);
        }
    }

    internal class GetInvalidOrdersQueries : TheoryData<GetCollectionProjectQuery, string, short>
    {
        public GetInvalidOrdersQueries()
        {
            var query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                orders: [""]);
            Add(query, nameof(query.Orders), 1);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                orders: ["something"]);
            Add(query, nameof(query.Orders), 1);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                orders: ["something,another"]);
            Add(query, nameof(query.Orders), 1);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                orders: ["something."]);
            Add(query, nameof(query.Orders), 2);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                orders: ["something.descending"]);
            Add(query, nameof(query.Orders), 2);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                orders: ["LastActivity.desc"]);
            Add(query, nameof(query.Orders), 1);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                orders: ["last_activity.desc", "name.ascending"]);
            Add(query, nameof(query.Orders), 1);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                orders: ["not_a_property.asc"]);
            Add(query, nameof(query.Orders), 1);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                orders: ["name.asc", "not_property.desc"]);
            Add(query, nameof(query.Orders), 1);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                orders: ["name.asc", "name.desc"]);
            Add(query, nameof(query.Orders), 1);
        }
    }

    internal class GetInvalidNameQueries : TheoryData<GetCollectionProjectQuery, string, short>
    {
        public GetInvalidNameQueries()
        {
            var query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                name: new string('*', PropertiesValidation.Project.NameMaximumLength + 1));
            Add(query, nameof(query.Name), 1);
        }
    }
}
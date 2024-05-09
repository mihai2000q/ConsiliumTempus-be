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
                pageSize: 10,
                currentPage: 2,
                order: "name.desc",
                workspaceId: Guid.NewGuid(),
                name: "Project",
                isFavorite: false,
                isPrivate: true);
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
    
    internal class GetInvalidOrderQueries : TheoryData<GetCollectionProjectQuery, string, short>
    {
        public GetInvalidOrderQueries()
        {
            var query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                order: "");
            Add(query, nameof(query.Order), 1);
            
            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                order: "something");
            Add(query, nameof(query.Order), 1);
            
            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                order: "something.");
            Add(query, nameof(query.Order), 2);
            
            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                order: "something.descending");
            Add(query, nameof(query.Order), 2);
            
            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                order: "LastActivity.desc");
            Add(query, nameof(query.Order), 1);
            
            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                order: "not_a_property.asc");
            Add(query, nameof(query.Order), 1);
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
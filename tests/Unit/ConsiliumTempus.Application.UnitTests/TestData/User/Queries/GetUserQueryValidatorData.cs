using ConsiliumTempus.Application.User.Queries.Get;
using ConsiliumTempus.Common.UnitTests.User;

namespace ConsiliumTempus.Application.UnitTests.TestData.User.Queries;

internal static class GetUserQueryValidatorData
{
    internal class GetValidQueries : TheoryData<GetUserQuery>
    {
        public GetValidQueries()
        {
            var query = UserQueryFactory.CreateGetUserQuery();
            Add(query);
            
            query = new GetUserQuery(Guid.NewGuid());
            Add(query);
        }
    }
    
    internal class GetInvalidIdQueries : TheoryData<GetUserQuery, string>
    {
        public GetInvalidIdQueries()
        {
            var query = UserQueryFactory.CreateGetUserQuery(id: Guid.Empty);
            Add(query, nameof(query.Id));
        }
    }
}
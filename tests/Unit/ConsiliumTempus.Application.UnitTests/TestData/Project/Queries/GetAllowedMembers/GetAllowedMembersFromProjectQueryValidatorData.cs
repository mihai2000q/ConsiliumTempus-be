using ConsiliumTempus.Application.Project.Queries.GetAllowedMembers;
using ConsiliumTempus.Common.UnitTests.Project;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Queries.GetAllowedMembers;

internal static class GetAllowedMembersFromProjectQueryValidatorData
{
    internal class GetValidQueries : TheoryData<GetAllowedMembersFromProjectQuery>
    {
        public GetValidQueries()
        {
            var query = ProjectQueryFactory.CreateGetAllowedMembersFromProjectQuery();
            Add(query);

            query = new GetAllowedMembersFromProjectQuery(Guid.NewGuid());
            Add(query);
        }
    }

    internal class GetInvalidIdQueries : TheoryData<GetAllowedMembersFromProjectQuery, string>
    {
        public GetInvalidIdQueries()
        {
            var query = ProjectQueryFactory.CreateGetAllowedMembersFromProjectQuery(id: Guid.Empty);
            Add(query, nameof(query.Id));
        }
    }
}
using ConsiliumTempus.Application.CustomFieldSetup.Queries.Get;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;

namespace ConsiliumTempus.Application.UnitTests.TestData.CustomFieldSetup.Queries;

public static class GetCustomFieldSetupQueryValidatorData
{
    internal class GetValidQueries : TheoryData<GetCustomFieldSetupQuery>
    {
        public GetValidQueries()
        {
            var query = CustomFieldSetupQueryFactory.CreateGetCustomFieldSetupQuery();
            Add(query);

            query = new GetCustomFieldSetupQuery(Guid.NewGuid());
            Add(query);
        }
    }

    internal class GetInvalidIdQueries : TheoryData<GetCustomFieldSetupQuery, string>
    {
        public GetInvalidIdQueries()
        {
            var query = CustomFieldSetupQueryFactory.CreateGetCustomFieldSetupQuery(Guid.Empty);
            Add(query, nameof(query.Id));
        }
    }
}
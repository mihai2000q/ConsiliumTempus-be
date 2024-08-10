using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.CustomFieldSetup.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;

namespace ConsiliumTempus.Application.UnitTests.TestData.CustomFieldSetup.Queries;

public static class GetCollectionCustomFieldSetupQueryValidatorData
{
    internal class GetValidQueries : TheoryData<GetCollectionCustomFieldSetupQuery>
    {
        public GetValidQueries()
        {
            var query = CustomFieldSetupQueryFactory.CreateGetCollectionCustomFieldSetupQuery(Guid.NewGuid());
            Add(query);

            query = new GetCollectionCustomFieldSetupQuery(
                Guid.NewGuid(),
                null);
            Add(query);
        }
    }

    internal class GetInvalidWorkspaceIdAndProjectIdQueries : TheoryData<GetCollectionCustomFieldSetupQuery, string>
    {
        public GetInvalidWorkspaceIdAndProjectIdQueries()
        {
            var query = CustomFieldSetupQueryFactory.CreateGetCollectionCustomFieldSetupQuery();
            Add(query, nameof(query.WorkspaceId).Dot(nameof(query.ProjectId)));
        }
    }
}
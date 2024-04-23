using ConsiliumTempus.Application.Project.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Domain.Common.Validation;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Queries.GetCollection;

internal static class GetCollectionProjectForWorkspaceQueryValidatorData
{
    internal class GetValidQueries : TheoryData<GetCollectionProjectQuery>
    {
        public GetValidQueries()
        {
            var query = ProjectQueryFactory.CreateGetCollectionProjectQuery();
            Add(query);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                workspaceId: Guid.NewGuid(),
                name: "Project",
                isFavorite: false,
                isPrivate: true);
            Add(query);
        }
    }

    internal class GetInvalidNameQueries : TheoryData<GetCollectionProjectQuery, string>
    {
        public GetInvalidNameQueries()
        {
            var query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                name: new string('*', PropertiesValidation.Project.NameMaximumLength + 1));
            Add(query, nameof(query.Name));
        }
    }
}
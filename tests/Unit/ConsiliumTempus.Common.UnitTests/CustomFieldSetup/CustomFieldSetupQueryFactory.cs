using ConsiliumTempus.Application.CustomFieldSetup.Queries.GetCollection;

namespace ConsiliumTempus.Common.UnitTests.CustomFieldSetup;

public static class CustomFieldSetupQueryFactory
{
    public static GetCollectionCustomFieldSetupQuery CreateGetCollectionCustomFieldSetupQuery(
        Guid? workspaceId = null,
        Guid? projectId = null)
    {
        return new GetCollectionCustomFieldSetupQuery(
            workspaceId,
            projectId);
    }
}
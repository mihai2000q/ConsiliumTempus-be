using ConsiliumTempus.Application.CustomFieldSetup.Queries.Get;
using ConsiliumTempus.Application.CustomFieldSetup.Queries.GetCollection;

namespace ConsiliumTempus.Common.UnitTests.CustomFieldSetup;

public static class CustomFieldSetupQueryFactory
{
    public static GetCustomFieldSetupQuery CreateGetCustomFieldSetupQuery(Guid? id = null)
    {
        return new GetCustomFieldSetupQuery(id ?? Guid.NewGuid());
    }

    public static GetCollectionCustomFieldSetupQuery CreateGetCollectionCustomFieldSetupQuery(
        Guid? workspaceId = null,
        Guid? projectId = null)
    {
        return new GetCollectionCustomFieldSetupQuery(
            workspaceId,
            projectId);
    }
}
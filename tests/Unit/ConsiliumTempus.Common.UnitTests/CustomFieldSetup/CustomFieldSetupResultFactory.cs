using ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Delete;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.UpdateWorkspace;
using ConsiliumTempus.Application.CustomFieldSetup.Queries.GetCollection;
using ConsiliumTempus.Domain.CustomFieldSetup;

namespace ConsiliumTempus.Common.UnitTests.CustomFieldSetup;

public static class CustomFieldSetupResultFactory
{
    public static GetCollectionCustomFieldSetupResult CreateGetCollectionCustomFieldSetupResult(
        List<CustomFieldSetupAggregate>? customFieldSetups = null)
    {
        return new GetCollectionCustomFieldSetupResult(
            customFieldSetups ?? CustomFieldSetupFactory.CreateList());
    }

    public static CreateCustomFieldSetupResult CreateCreateCustomFieldSetupResult()
    {
        return new CreateCustomFieldSetupResult();
    }

    public static UpdateWorkspaceCustomFieldSetupResult CreateUpdateWorkspaceCustomFieldSetupResult()
    {
        return new UpdateWorkspaceCustomFieldSetupResult();
    }

    public static DeleteCustomFieldSetupResult CreateDeleteCustomFieldSetupResult()
    {
        return new DeleteCustomFieldSetupResult();
    }
}
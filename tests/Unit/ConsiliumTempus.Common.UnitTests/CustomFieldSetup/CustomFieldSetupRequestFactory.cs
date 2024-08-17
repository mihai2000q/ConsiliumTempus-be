using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Delete;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Get;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromProject;
using ConsiliumTempus.Common.UnitTests.TestConstants;
using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Common.UnitTests.CustomFieldSetup;

public static class CustomFieldSetupRequestFactory
{
    public static GetCustomFieldSetupRequest CreateGetCustomFieldSetupRequest(Guid? id = null)
    {
        return new GetCustomFieldSetupRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }

    public static GetCollectionCustomFieldSetupFromProjectRequest CreateGetCollectionCustomFieldSetupFromProjectRequest(
        Guid? projectId = null)
    {
        return new GetCollectionCustomFieldSetupFromProjectRequest
        {
            ProjectId = projectId ?? Guid.NewGuid()
        };
    }

    public static CreateCustomFieldSetupOnProjectRequest CreateCreateCustomFieldSetupOnProjectRequest(
        Guid? projectId = null,
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description,
        CustomFieldType type = CustomFieldType.Text,
        CreateCustomFieldSetupOnProjectRequest.CreateNumberCustomFieldSetupRequest? numberCustomFieldSetup = null,
        CreateCustomFieldSetupOnProjectRequest.CreateSingleSelectCustomFieldSetupRequest? singleSelectCustomFieldSetup = null,
        CreateCustomFieldSetupOnProjectRequest.CreateTextCustomFieldSetupRequest? textCustomFieldSetup = null)
    {
        return new CreateCustomFieldSetupOnProjectRequest(
            projectId,
            name,
            description,
            type.ToString(),
            numberCustomFieldSetup,
            singleSelectCustomFieldSetup,
            textCustomFieldSetup);
    }

    public static DeleteCustomFieldSetupRequest CreateDeleteCustomFieldSetupRequest(Guid? id = null)
    {
        return new DeleteCustomFieldSetupRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }
}
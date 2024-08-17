using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Get;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromProject;
using ConsiliumTempus.Common.IntegrationTests.TestConstants;
using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup;

public static class CustomFieldSetupRequestFactory
{
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
}
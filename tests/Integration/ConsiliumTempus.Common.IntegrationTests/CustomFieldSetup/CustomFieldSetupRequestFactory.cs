using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnProject;
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
        CreateCustomFieldSetupOnProjectRequest.NumberSettingsRequest? numberSettings = null,
        List<CreateCustomFieldSetupOnProjectRequest.SingleSelectOptionRequest>? singleSelectOptions = null)
    {
        return new CreateCustomFieldSetupOnProjectRequest(
            projectId,
            name,
            description,
            type.ToString(),
            numberSettings,
            singleSelectOptions);
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
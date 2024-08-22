using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Delete;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Get;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromWorkspace;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.UpdateWorkspace;
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
            type,
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

    public static GetCollectionCustomFieldSetupFromWorkspaceRequest CreateGetCollectionCustomFieldSetupFromWorkspaceRequest(
        Guid? workspaceId = null)
    {
        return new GetCollectionCustomFieldSetupFromWorkspaceRequest
        {
            WorkspaceId = workspaceId ?? Guid.NewGuid()
        };
    }

    public static UpdateWorkspaceCustomFieldSetupRequest CreateUpdateWorkspaceCustomFieldSetupRequest(
        Guid? id = null,
        Guid? workspaceId = null)
    {
        return new UpdateWorkspaceCustomFieldSetupRequest(
            id ?? Guid.NewGuid(),
            workspaceId ?? Guid.NewGuid());
    }
}
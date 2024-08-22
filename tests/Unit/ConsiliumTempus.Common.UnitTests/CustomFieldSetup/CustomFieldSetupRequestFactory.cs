using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Create;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnWorkspace;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Delete;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Get;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromWorkspace;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.UpdateWorkspace;
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

    public static GetCollectionCustomFieldSetupFromWorkspaceRequest CreateGetCollectionCustomFieldSetupFromWorkspaceRequest(
        Guid? workspaceId = null)
    {
        return new GetCollectionCustomFieldSetupFromWorkspaceRequest
        {
            WorkspaceId = workspaceId ?? Guid.NewGuid()
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

    public static CreateCustomFieldSetupOnWorkspaceRequest CreateCreateCustomFieldSetupOnWorkspaceRequest(
        Guid? workspaceId = null,
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description,
        CustomFieldType type = CustomFieldType.Text,
        CreateCustomFieldSetupRequest.NumberCustomFieldSetupRequest? numberCustomFieldSetup = null,
        CreateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest? singleSelectCustomFieldSetup = null,
        CreateCustomFieldSetupRequest.TextCustomFieldSetupRequest? textCustomFieldSetup = null)
    {
        return new CreateCustomFieldSetupOnWorkspaceRequest(
            workspaceId ?? Guid.NewGuid(),
            name,
            description,
            type,
            numberCustomFieldSetup,
            singleSelectCustomFieldSetup,
            textCustomFieldSetup);
    }

    public static CreateCustomFieldSetupOnProjectRequest CreateCreateCustomFieldSetupOnProjectRequest(
        Guid? projectId = null,
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description,
        CustomFieldType type = CustomFieldType.Text,
        CreateCustomFieldSetupRequest.NumberCustomFieldSetupRequest? numberCustomFieldSetup = null,
        CreateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest? singleSelectCustomFieldSetup = null,
        CreateCustomFieldSetupRequest.TextCustomFieldSetupRequest? textCustomFieldSetup = null)
    {
        return new CreateCustomFieldSetupOnProjectRequest(
            projectId ?? Guid.NewGuid(),
            name,
            description,
            type,
            numberCustomFieldSetup,
            singleSelectCustomFieldSetup,
            textCustomFieldSetup);
    }
    
    public static UpdateWorkspaceCustomFieldSetupRequest CreateUpdateWorkspaceCustomFieldSetupRequest(
        Guid? id = null,
        Guid? workspaceId = null)
    {
        return new UpdateWorkspaceCustomFieldSetupRequest(
            id ?? Guid.NewGuid(),
            workspaceId ?? Guid.NewGuid());
    }

    public static DeleteCustomFieldSetupRequest CreateDeleteCustomFieldSetupRequest(Guid? id = null)
    {
        return new DeleteCustomFieldSetupRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }
}
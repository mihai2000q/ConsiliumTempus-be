using ConsiliumTempus.Api.Contracts.CustomFieldSetup.AddToProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Create;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnWorkspace;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Delete;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Get;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromWorkspace;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.MakeGlobal;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.RemoveFromProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Update;
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

    public static GetCollectionCustomFieldSetupFromWorkspaceRequest
        CreateGetCollectionCustomFieldSetupFromWorkspaceRequest(
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
        CreateCustomFieldSetupRequest.DateCustomFieldSetupRequest? dateCustomFieldSetup = null,
        CreateCustomFieldSetupRequest.DateTimeCustomFieldSetupRequest? dateTimeCustomFieldSetup = null,
        CreateCustomFieldSetupRequest.DurationCustomFieldSetupRequest? durationCustomFieldSetup = null,
        CreateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest? multiSelectCustomFieldSetup = null,
        CreateCustomFieldSetupRequest.NumberCustomFieldSetupRequest? numberCustomFieldSetup = null,
        CreateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest? singleSelectCustomFieldSetup = null,
        CreateCustomFieldSetupRequest.TextCustomFieldSetupRequest? textCustomFieldSetup = null,
        CreateCustomFieldSetupRequest.TimeCustomFieldSetupRequest? timeCustomFieldSetup = null)
    {
        return new CreateCustomFieldSetupOnWorkspaceRequest(
            workspaceId ?? Guid.NewGuid(),
            name,
            description,
            type,
            dateCustomFieldSetup, 
            dateTimeCustomFieldSetup, 
            durationCustomFieldSetup,
            multiSelectCustomFieldSetup, 
            numberCustomFieldSetup, 
            singleSelectCustomFieldSetup, 
            textCustomFieldSetup,
            timeCustomFieldSetup);
    }

    public static CreateCustomFieldSetupOnProjectRequest CreateCreateCustomFieldSetupOnProjectRequest(
        Guid? projectId = null,
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description,
        CustomFieldType type = CustomFieldType.Text,
        CreateCustomFieldSetupRequest.DateCustomFieldSetupRequest? dateCustomFieldSetup = null,
        CreateCustomFieldSetupRequest.DateTimeCustomFieldSetupRequest? dateTimeCustomFieldSetup = null,
        CreateCustomFieldSetupRequest.DurationCustomFieldSetupRequest? durationCustomFieldSetup = null,
        CreateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest? multiSelectCustomFieldSetup = null,
        CreateCustomFieldSetupRequest.NumberCustomFieldSetupRequest? numberCustomFieldSetup = null,
        CreateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest? singleSelectCustomFieldSetup = null,
        CreateCustomFieldSetupRequest.TextCustomFieldSetupRequest? textCustomFieldSetup = null,
        CreateCustomFieldSetupRequest.TimeCustomFieldSetupRequest? timeCustomFieldSetup = null)
    {
        return new CreateCustomFieldSetupOnProjectRequest(
            projectId ?? Guid.NewGuid(),
            name,
            description,
            type,
            dateCustomFieldSetup, 
            dateTimeCustomFieldSetup, 
            durationCustomFieldSetup,
            multiSelectCustomFieldSetup, 
            numberCustomFieldSetup, 
            singleSelectCustomFieldSetup, 
            textCustomFieldSetup,
            timeCustomFieldSetup);
    }

    public static AddCustomFieldSetupToProjectRequest CreateAddCustomFieldSetupToProjectRequest(
        Guid? id = null,
        Guid? projectId = null)
    {
        return new AddCustomFieldSetupToProjectRequest(
            id ?? Guid.NewGuid(),
            projectId ?? Guid.NewGuid());
    }

    public static UpdateCustomFieldSetupRequest CreateUpdateCustomFieldSetupRequest(
        Guid? id = null,
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description,
        CustomFieldType type = CustomFieldType.Text,
        UpdateCustomFieldSetupRequest.NumberCustomFieldSetupRequest? numberCustomFieldSetup = null,
        UpdateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest? singleSelectCustomFieldSetup = null,
        UpdateCustomFieldSetupRequest.TextCustomFieldSetupRequest? textCustomFieldSetup = null)
    {
        return new UpdateCustomFieldSetupRequest(
            id ?? Guid.NewGuid(),
            name,
            description,
            type,
            numberCustomFieldSetup,
            singleSelectCustomFieldSetup,
            textCustomFieldSetup);
    }

    public static MakeCustomFieldSetupGlobalRequest CreateMakeCustomFieldSetupGlobalRequest(Guid? id = null)
    {
        return new MakeCustomFieldSetupGlobalRequest(id ?? Guid.NewGuid());
    }

    public static DeleteCustomFieldSetupRequest CreateDeleteCustomFieldSetupRequest(Guid? id = null)
    {
        return new DeleteCustomFieldSetupRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }

    public static RemoveCustomFieldSetupFromProjectRequest CreateRemoveCustomFieldSetupFromProjectRequest(
        Guid? id = null,
        Guid? projectId = null)
    {
        return new RemoveCustomFieldSetupFromProjectRequest
        {
            Id = id ?? Guid.NewGuid(),
            ProjectId = projectId ?? Guid.NewGuid()
        };
    }
}
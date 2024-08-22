using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Create;
using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnWorkspace;

public sealed record CreateCustomFieldSetupOnWorkspaceRequest(
    Guid WorkspaceId,
    string Name,
    string Description,
    CustomFieldType Type,
    CreateCustomFieldSetupRequest.NumberCustomFieldSetupRequest? NumberCustomFieldSetup,
    CreateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest? SingleSelectCustomFieldSetup,
    CreateCustomFieldSetupRequest.TextCustomFieldSetupRequest? TextCustomFieldSetup)
    : CreateCustomFieldSetupRequest(
        Name,
        Description,
        Type,
        NumberCustomFieldSetup,
        SingleSelectCustomFieldSetup, 
        TextCustomFieldSetup);
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Create;
using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnProject;

public sealed record CreateCustomFieldSetupOnProjectRequest(
    Guid ProjectId,
    string Name,
    string Description,
    CustomFieldType Type,
    CreateCustomFieldSetupRequest.DateCustomFieldSetupRequest? DateCustomFieldSetup,
    CreateCustomFieldSetupRequest.DateTimeCustomFieldSetupRequest? DateTimeCustomFieldSetup,
    CreateCustomFieldSetupRequest.DurationCustomFieldSetupRequest? DurationCustomFieldSetup,
    CreateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest? MultiSelectCustomFieldSetup,
    CreateCustomFieldSetupRequest.NumberCustomFieldSetupRequest? NumberCustomFieldSetup,
    CreateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest? SingleSelectCustomFieldSetup,
    CreateCustomFieldSetupRequest.TextCustomFieldSetupRequest? TextCustomFieldSetup,
    CreateCustomFieldSetupRequest.TimeCustomFieldSetupRequest? TimeCustomFieldSetup)
    : CreateCustomFieldSetupRequest(
        Name,
        Description,
        Type,
        DateCustomFieldSetup,
        DateTimeCustomFieldSetup, 
        DurationCustomFieldSetup, 
        MultiSelectCustomFieldSetup,
        NumberCustomFieldSetup, 
        SingleSelectCustomFieldSetup, 
        TextCustomFieldSetup, 
        TimeCustomFieldSetup);
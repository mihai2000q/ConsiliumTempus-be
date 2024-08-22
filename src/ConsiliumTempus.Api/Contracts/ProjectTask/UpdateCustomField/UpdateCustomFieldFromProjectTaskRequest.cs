using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Api.Contracts.ProjectTask.UpdateCustomField;

public sealed record UpdateCustomFieldFromProjectTaskRequest(
    Guid Id,
    Guid CustomFieldId,
    CustomFieldType Type,
    UpdateCustomFieldFromProjectTaskRequest.NumberCustomFieldRequest? NumberCustomField,
    UpdateCustomFieldFromProjectTaskRequest.SingleSelectCustomFieldRequest? SingleSelectCustomField,
    UpdateCustomFieldFromProjectTaskRequest.TextCustomFieldRequest? TextCustomField)
{
    public sealed record NumberCustomFieldRequest(
        decimal? Number);

    public sealed record SingleSelectCustomFieldRequest(
        Guid? OptionId);

    public sealed record TextCustomFieldRequest(
        string? Text);
}
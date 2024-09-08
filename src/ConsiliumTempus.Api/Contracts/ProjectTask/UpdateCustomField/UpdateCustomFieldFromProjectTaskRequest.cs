using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Api.Contracts.ProjectTask.UpdateCustomField;

public sealed record UpdateCustomFieldFromProjectTaskRequest(
    Guid Id,
    Guid CustomFieldId,
    CustomFieldType Type,
    UpdateCustomFieldFromProjectTaskRequest.DateCustomFieldRequest? DateCustomField,
    UpdateCustomFieldFromProjectTaskRequest.DateTimeCustomFieldRequest? DateTimeCustomField,
    UpdateCustomFieldFromProjectTaskRequest.DurationCustomFieldRequest? DurationCustomField,
    UpdateCustomFieldFromProjectTaskRequest.MultiSelectCustomFieldRequest? MultiSelectCustomField,
    UpdateCustomFieldFromProjectTaskRequest.NumberCustomFieldRequest? NumberCustomField,
    UpdateCustomFieldFromProjectTaskRequest.PeopleCustomFieldRequest? PeopleCustomField,
    UpdateCustomFieldFromProjectTaskRequest.SingleSelectCustomFieldRequest? SingleSelectCustomField,
    UpdateCustomFieldFromProjectTaskRequest.TextCustomFieldRequest? TextCustomField,
    UpdateCustomFieldFromProjectTaskRequest.TimeCustomFieldRequest? TimeCustomField)
{
    public sealed record DateCustomFieldRequest(
        DateOnly? Date);

    public sealed record DateTimeCustomFieldRequest(
        DateTime? DateTime);

    public sealed record DurationCustomFieldRequest(
        TimeSpan? Duration);

    public sealed record MultiSelectCustomFieldRequest(
        Guid OptionId,
        bool Remove);

    public sealed record NumberCustomFieldRequest(
        decimal? Number);

    public sealed record PeopleCustomFieldRequest(
        Guid? PersonId);

    public sealed record SingleSelectCustomFieldRequest(
        Guid? OptionId);

    public sealed record TextCustomFieldRequest(
        string? Text);

    public sealed record TimeCustomFieldRequest(
        TimeOnly? Time);
}
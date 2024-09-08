using ConsiliumTempus.Domain.Common.Enums;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectTask.Commands.UpdateCustomField;

public sealed record UpdateCustomFieldFromProjectTaskCommand(
    Guid Id,
    Guid CustomFieldId,
    CustomFieldType Type,
    UpdateCustomFieldFromProjectTaskCommand.DateCustomFieldCommand? DateCustomField,
    UpdateCustomFieldFromProjectTaskCommand.DateTimeCustomFieldCommand? DateTimeCustomField,
    UpdateCustomFieldFromProjectTaskCommand.DurationCustomFieldCommand? DurationCustomField,
    UpdateCustomFieldFromProjectTaskCommand.MultiSelectCustomFieldCommand? MultiSelectCustomField,
    UpdateCustomFieldFromProjectTaskCommand.NumberCustomFieldCommand? NumberCustomField,
    UpdateCustomFieldFromProjectTaskCommand.PeopleCustomFieldCommand? PeopleCustomField,
    UpdateCustomFieldFromProjectTaskCommand.SingleSelectCustomFieldCommand? SingleSelectCustomField,
    UpdateCustomFieldFromProjectTaskCommand.TextCustomFieldCommand? TextCustomField,
    UpdateCustomFieldFromProjectTaskCommand.TimeCustomFieldCommand? TimeCustomField)
    : IRequest<ErrorOr<UpdateCustomFieldFromProjectTaskResult>>
{
    public sealed record DateCustomFieldCommand(
        DateOnly? Date);

    public sealed record DateTimeCustomFieldCommand(
        DateTime? DateTime);

    public sealed record DurationCustomFieldCommand(
        TimeSpan? Duration);

    public sealed record MultiSelectCustomFieldCommand(
        Guid OptionId,
        bool Remove);

    public sealed record NumberCustomFieldCommand(
        decimal? Number);

    public sealed record PeopleCustomFieldCommand(
        Guid? PersonId);

    public sealed record SingleSelectCustomFieldCommand(
        Guid? OptionId);

    public sealed record TextCustomFieldCommand(
        string? Text);

    public sealed record TimeCustomFieldCommand(
        TimeOnly? Time);
}
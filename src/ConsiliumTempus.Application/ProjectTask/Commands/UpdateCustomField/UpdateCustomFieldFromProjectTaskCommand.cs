using ConsiliumTempus.Domain.Common.Enums;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectTask.Commands.UpdateCustomField;

public sealed record UpdateCustomFieldFromProjectTaskCommand(
    Guid Id,
    Guid CustomFieldId,
    CustomFieldType Type,
    UpdateCustomFieldFromProjectTaskCommand.NumberCustomFieldCommand? NumberCustomField,
    UpdateCustomFieldFromProjectTaskCommand.SingleSelectCustomFieldCommand? SingleSelectCustomField,
    UpdateCustomFieldFromProjectTaskCommand.TextCustomFieldCommand? TextCustomField)
    : IRequest<ErrorOr<UpdateCustomFieldFromProjectTaskResult>>
{
    public sealed record NumberCustomFieldCommand(
        decimal? Number);

    public sealed record SingleSelectCustomFieldCommand(
        Guid? OptionId);

    public sealed record TextCustomFieldCommand(
        string? Text);
}
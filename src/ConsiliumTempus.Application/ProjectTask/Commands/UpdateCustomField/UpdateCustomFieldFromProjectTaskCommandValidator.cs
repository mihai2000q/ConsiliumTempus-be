using ConsiliumTempus.Domain.Common.Enums;
using FluentValidation;

namespace ConsiliumTempus.Application.ProjectTask.Commands.UpdateCustomField;

public sealed class UpdateCustomFieldFromProjectTaskCommandValidator
    : AbstractValidator<UpdateCustomFieldFromProjectTaskCommand>
{
    public UpdateCustomFieldFromProjectTaskCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.CustomFieldId)
            .NotEmpty();

        When(c => c.Type == CustomFieldType.Date, () =>
        {
            RuleFor(c => c.DateCustomField)
                .NotNull();
        });

        When(c => c.Type == CustomFieldType.DateTime, () =>
        {
            RuleFor(c => c.DateTimeCustomField)
                .NotNull();
        });

        When(c => c.Type == CustomFieldType.Duration, () =>
        {
            RuleFor(c => c.DurationCustomField)
                .NotNull();
        });

        When(c => c.Type == CustomFieldType.MultiSelect, () =>
        {
            RuleFor(c => c.MultiSelectCustomField)
                .NotNull();

            RuleFor(c => c.MultiSelectCustomField!.OptionId)
                .NotEmpty()
                .When(c => c.MultiSelectCustomField != null);
        });

        When(c => c.Type == CustomFieldType.Number, () =>
        {
            RuleFor(c => c.NumberCustomField)
                .NotNull();
        });

        When(c => c.Type == CustomFieldType.People, () =>
        {
            RuleFor(c => c.PeopleCustomField)
                .NotNull();
        });

        When(c => c.Type == CustomFieldType.SingleSelect, () =>
        {
            RuleFor(c => c.SingleSelectCustomField)
                .NotNull();
        });

        When(c => c.Type == CustomFieldType.Text, () =>
        {
            RuleFor(c => c.TextCustomField)
                .NotNull();
        });

        When(c => c.Type == CustomFieldType.Time, () =>
        {
            RuleFor(c => c.TimeCustomField)
                .NotNull();
        });
    }
}
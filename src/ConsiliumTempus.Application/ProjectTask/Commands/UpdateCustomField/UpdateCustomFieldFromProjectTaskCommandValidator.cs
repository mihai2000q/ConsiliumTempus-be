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

        When(c => c.Type == CustomFieldType.Number, () =>
        {
            RuleFor(c => c.NumberCustomField)
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
    }
}
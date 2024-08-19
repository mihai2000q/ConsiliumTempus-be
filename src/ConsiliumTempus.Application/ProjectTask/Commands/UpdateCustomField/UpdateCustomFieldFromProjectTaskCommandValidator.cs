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

        RuleFor(c => c.Type)
            .IsEnumName(typeof(CustomFieldType));

        When(c => c.Type == CustomFieldType.Number.ToString(), () =>
        {
            RuleFor(c => c.NumberCustomField)
                .NotNull();
        });

        When(c => c.Type == CustomFieldType.SingleSelect.ToString(), () =>
        {
            RuleFor(c => c.SingleSelectCustomField)
                .NotNull();
        });

        When(c => c.Type == CustomFieldType.Text.ToString(), () =>
        {
            RuleFor(c => c.TextCustomField)
                .NotNull();
        });
    }
}
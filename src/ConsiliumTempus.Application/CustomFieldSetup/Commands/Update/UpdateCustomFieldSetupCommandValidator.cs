using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Validation;
using FluentValidation;

namespace ConsiliumTempus.Application.CustomFieldSetup.Commands.Update;

public sealed class UpdateCustomFieldSetupCommandValidator : AbstractValidator<UpdateCustomFieldSetupCommand>
{
    public UpdateCustomFieldSetupCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(PropertiesValidation.CustomFieldSetup.NameMaximumLength);
        
        RuleFor(c => c.Type)
            .IsEnumName(typeof(CustomFieldType));

        // Number Custom Field Setup
        When(c => c.Type.Equals(CustomFieldType.Number.ToString()),
            () =>
            {
                RuleFor(c => c.NumberCustomFieldSetup)
                    .NotNull();

                When(c => c.NumberCustomFieldSetup is not null, () =>
                {
                    RuleFor(c => c.NumberCustomFieldSetup!.Settings.CurrencyCode)
                        .IsCurrencyCode();

                    RuleFor(c => c.NumberCustomFieldSetup!.Settings.Decimals)
                        .GreaterThanOrEqualTo(0)
                        .LessThanOrEqualTo(PropertiesValidation.CustomFieldSetup.Number.DecimalsMaximum);
                });
            });

        // Single Select Custom Field Setup
        When(c => c.Type.Equals(CustomFieldType.SingleSelect.ToString()),
            () =>
            {
                RuleFor(c => c.SingleSelectCustomFieldSetup)
                    .NotNull();
            });

        // Text Custom Field Setup
        When(c => c.Type.Equals(CustomFieldType.Text.ToString()),
            () =>
            {
                RuleFor(c => c.TextCustomFieldSetup)
                    .NotNull();
            });
    }
}
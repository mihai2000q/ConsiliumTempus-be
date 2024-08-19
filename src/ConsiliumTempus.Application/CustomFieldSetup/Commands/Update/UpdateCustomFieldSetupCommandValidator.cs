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
        When(c => c.Type == CustomFieldType.Number.ToString(),
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
        When(c => c.Type == CustomFieldType.SingleSelect.ToString(),
            () =>
            {
                RuleFor(c => c.SingleSelectCustomFieldSetup)
                    .NotNull();

                When(c => c.SingleSelectCustomFieldSetup != null, () =>
                {
                    RuleFor(c => c.SingleSelectCustomFieldSetup!.Operation)
                        .IsEnumName(typeof(UpdateCustomFieldSetupCommand.SingleSelectOptionOperation))
                        .When(c => c.SingleSelectCustomFieldSetup!.Operation is not null);
                    
                    // Update or Add
                    RuleFor(c => c.SingleSelectCustomFieldSetup!.NewOption!.Value)
                        .NotEmpty()
                        .MaximumLength(PropertiesValidation.SingleSelectOption.ValueMaximumLength)
                        .When(c => c.SingleSelectCustomFieldSetup!.NewOption != null);

                    RuleFor(c => c.SingleSelectCustomFieldSetup!.NewOption!.Color)
                        .IsColor()
                        .When(c => c.SingleSelectCustomFieldSetup!.NewOption != null);

                    // Add
                    When(
                        c => c.SingleSelectCustomFieldSetup!.Operation == UpdateCustomFieldSetupCommand
                            .SingleSelectOptionOperation.Add.ToString(),
                        () =>
                        {
                            RuleFor(c => c.SingleSelectCustomFieldSetup!.NewOption)
                                .NotNull();
                        });

                    // Update
                    When(
                        c => c.SingleSelectCustomFieldSetup!.Operation == UpdateCustomFieldSetupCommand
                            .SingleSelectOptionOperation.Update.ToString(),
                        () =>
                        {
                            RuleFor(c => c.SingleSelectCustomFieldSetup!.NewOption)
                                .NotNull();

                            RuleFor(c => c.SingleSelectCustomFieldSetup!.OptionId)
                                .NotEmpty();
                        });

                    // Move
                    When(
                        c => c.SingleSelectCustomFieldSetup!.Operation == UpdateCustomFieldSetupCommand
                            .SingleSelectOptionOperation.Move.ToString(),
                        () =>
                        {
                            RuleFor(c => c.SingleSelectCustomFieldSetup!.OptionId)
                                .NotEmpty();

                            RuleFor(c => c.SingleSelectCustomFieldSetup!.OverOptionId)
                                .NotEmpty();
                        });

                    // Remove
                    When(
                        c => c.SingleSelectCustomFieldSetup!.Operation == UpdateCustomFieldSetupCommand
                            .SingleSelectOptionOperation.Remove.ToString(),
                        () =>
                        {
                            RuleFor(c => c.SingleSelectCustomFieldSetup!.OptionId)
                                .NotEmpty();
                        });
                });
            });

        // Text Custom Field Setup
        When(c => c.Type == CustomFieldType.Text.ToString(),
            () =>
            {
                RuleFor(c => c.TextCustomFieldSetup)
                    .NotNull();
            });
    }
}
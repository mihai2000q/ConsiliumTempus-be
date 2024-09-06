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

        // Date Custom Field Setup
        When(c => c.Type == CustomFieldType.Date,
            () =>
            {
                RuleFor(c => c.DateCustomFieldSetup)
                    .NotNull();
            });

        // Date Time Custom Field Setup
        When(c => c.Type == CustomFieldType.DateTime,
            () =>
            {
                RuleFor(c => c.DateTimeCustomFieldSetup)
                    .NotNull();
            });

        // Duration Custom Field Setup
        When(c => c.Type == CustomFieldType.Duration,
            () =>
            {
                RuleFor(c => c.DurationCustomFieldSetup)
                    .NotNull();
            });

        // Multi Select Custom Field Setup
        When(c => c.Type == CustomFieldType.MultiSelect,
            () =>
            {
                RuleFor(c => c.MultiSelectCustomFieldSetup)
                    .NotNull();

                When(c => c.MultiSelectCustomFieldSetup != null, () =>
                {
                    // Update or Add
                    RuleFor(c => c.MultiSelectCustomFieldSetup!.NewOption!.Value)
                        .NotEmpty()
                        .MaximumLength(PropertiesValidation.SingleSelectOption.ValueMaximumLength)
                        .When(c => c.MultiSelectCustomFieldSetup!.NewOption != null);

                    RuleFor(c => c.MultiSelectCustomFieldSetup!.NewOption!.Color)
                        .IsColor()
                        .When(c => c.MultiSelectCustomFieldSetup!.NewOption != null);

                    // Add
                    When(
                        c => c.MultiSelectCustomFieldSetup!.Operation == UpdateCustomFieldSetupCommand
                            .OptionOperation.Add,
                        () =>
                        {
                            RuleFor(c => c.MultiSelectCustomFieldSetup!.NewOption)
                                .NotNull();
                        });

                    // Update
                    When(
                        c => c.MultiSelectCustomFieldSetup!.Operation == UpdateCustomFieldSetupCommand
                            .OptionOperation.Update,
                        () =>
                        {
                            RuleFor(c => c.MultiSelectCustomFieldSetup!.NewOption)
                                .NotNull();

                            RuleFor(c => c.MultiSelectCustomFieldSetup!.OptionId)
                                .NotEmpty();
                        });

                    // Move
                    When(
                        c => c.MultiSelectCustomFieldSetup!.Operation == UpdateCustomFieldSetupCommand
                            .OptionOperation.Move,
                        () =>
                        {
                            RuleFor(c => c.MultiSelectCustomFieldSetup!.OptionId)
                                .NotEmpty();

                            RuleFor(c => c.MultiSelectCustomFieldSetup!.OverOptionId)
                                .NotEmpty();
                        });

                    // Remove
                    When(
                        c => c.MultiSelectCustomFieldSetup!.Operation == UpdateCustomFieldSetupCommand
                            .OptionOperation.Remove,
                        () =>
                        {
                            RuleFor(c => c.MultiSelectCustomFieldSetup!.OptionId)
                                .NotEmpty();
                        });
                });
            });

        // Number Custom Field Setup
        When(c => c.Type == CustomFieldType.Number,
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
        When(c => c.Type == CustomFieldType.SingleSelect,
            () =>
            {
                RuleFor(c => c.SingleSelectCustomFieldSetup)
                    .NotNull();

                When(c => c.SingleSelectCustomFieldSetup != null, () =>
                {
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
                            .OptionOperation.Add,
                        () =>
                        {
                            RuleFor(c => c.SingleSelectCustomFieldSetup!.NewOption)
                                .NotNull();
                        });

                    // Update
                    When(
                        c => c.SingleSelectCustomFieldSetup!.Operation == UpdateCustomFieldSetupCommand
                            .OptionOperation.Update,
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
                            .OptionOperation.Move,
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
                            .OptionOperation.Remove,
                        () =>
                        {
                            RuleFor(c => c.SingleSelectCustomFieldSetup!.OptionId)
                                .NotEmpty();
                        });
                });
            });

        // Text Custom Field Setup
        When(c => c.Type == CustomFieldType.Text,
            () =>
            {
                RuleFor(c => c.TextCustomFieldSetup)
                    .NotNull();
            });

        // Time Custom Field Setup
        When(c => c.Type == CustomFieldType.Time,
            () =>
            {
                RuleFor(c => c.TimeCustomFieldSetup)
                    .NotNull();
            });
    }
}
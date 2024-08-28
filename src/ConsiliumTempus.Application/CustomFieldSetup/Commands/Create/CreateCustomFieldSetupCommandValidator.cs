using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Validation;
using FluentValidation;

namespace ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;

public sealed class CreateCustomFieldSetupCommandValidator : AbstractValidator<CreateCustomFieldSetupCommand>
{
    public CreateCustomFieldSetupCommandValidator()
    {
        RuleFor(c => c)
            .Must(c =>
                (c.WorkspaceId is not null && c.WorkspaceId != Guid.Empty) ||
                (c.ProjectId is not null && c.ProjectId != Guid.Empty))
            .WithMessage("Either the 'WorkspaceId' or the 'ProjectId' must be set.")
            .WithName(nameof(CreateCustomFieldSetupCommand.WorkspaceId)
                .Dot(nameof(CreateCustomFieldSetupCommand.ProjectId)));

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

                When(c => c.MultiSelectCustomFieldSetup is not null, () =>
                {
                    RuleForEach(c => c.MultiSelectCustomFieldSetup!.Options).ChildRules(option =>
                    {
                        option.RuleFor(o => o.Value)
                            .NotEmpty()
                            .MaximumLength(PropertiesValidation.MultiSelectOption.ValueMaximumLength);

                        option.RuleFor(o => o.Color)
                            .IsColor();
                    });

                    RuleFor(c => c.MultiSelectCustomFieldSetup!.Options)
                        .NotEmpty();
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

                When(c => c.SingleSelectCustomFieldSetup is not null, () =>
                {
                    RuleForEach(c => c.SingleSelectCustomFieldSetup!.Options).ChildRules(option =>
                    {
                        option.RuleFor(o => o.Id)
                            .NotEmpty();

                        option.RuleFor(o => o.Value)
                            .NotEmpty()
                            .MaximumLength(PropertiesValidation.SingleSelectOption.ValueMaximumLength);

                        option.RuleFor(o => o.Color)
                            .IsColor();
                    });

                    RuleFor(c => c.SingleSelectCustomFieldSetup!.Options)
                        .NotEmpty()
                        .Must(options =>
                            options.DistinctBy(o => o.Id).Count() == options.Count)
                        .WithMessage("{PropertyName} cannot have duplicate ids");

                    When(c => c.SingleSelectCustomFieldSetup!.DefaultOptionId is not null, () =>
                    {
                        RuleFor(c => c.SingleSelectCustomFieldSetup)
                            .Must(c => c!.Options
                                .FirstOrDefault(o => o.Id == c.DefaultOptionId) is not null)
                            .WithMessage("{PropertyName} could not be found within the provided options")
                            .OverridePropertyName(nameof(CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetup)
                                .Dot(nameof(CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetup
                                    .DefaultOptionId)));
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
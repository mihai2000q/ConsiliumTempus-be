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
                                .Dot(nameof(CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetup.DefaultOptionId)));
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
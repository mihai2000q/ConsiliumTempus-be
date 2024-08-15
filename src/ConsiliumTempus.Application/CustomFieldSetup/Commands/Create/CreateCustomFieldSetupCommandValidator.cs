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
            .IsEnumName(typeof(CustomFieldType), false);

        When(c => c.Type.Equals(CustomFieldType.Number.ToString(), StringComparison.CurrentCultureIgnoreCase),
            () =>
            {
                RuleFor(c => c.NumberSettings)
                    .NotNull();

                When(c => c.NumberSettings is not null, () =>
                {
                    RuleFor(c => c.NumberSettings!.CurrencyCode)
                        .IsCurrencyCode();

                    RuleFor(c => c.NumberSettings!.Decimals)
                        .GreaterThanOrEqualTo(0)
                        .LessThanOrEqualTo(PropertiesValidation.CustomFieldSetup.Number.DecimalsMaximum);
                });
            });

        When(c => c.Type.Equals(CustomFieldType.SingleSelect.ToString(), StringComparison.CurrentCultureIgnoreCase),
            () =>
            {
                RuleFor(c => c.SingleSelectOptions)
                    .NotEmpty();

                RuleForEach(c => c.SingleSelectOptions).ChildRules(option =>
                {
                    option.RuleFor(o => o.Value)
                        .NotEmpty()
                        .MaximumLength(PropertiesValidation.SingleSelectOption.ValueMaximumLength);

                    option.RuleFor(o => o.Color)
                        .IsColor();
                });
            });
    }
}
using FluentValidation;

namespace ConsiliumTempus.Application.CustomFieldSetup.Commands.Delete;

public sealed class DeleteCustomFieldSetupCommandValidator : AbstractValidator<DeleteCustomFieldSetupCommand>
{
    public DeleteCustomFieldSetupCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}
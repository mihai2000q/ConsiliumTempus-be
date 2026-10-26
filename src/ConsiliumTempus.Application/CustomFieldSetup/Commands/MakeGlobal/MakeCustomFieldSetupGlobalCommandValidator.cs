using FluentValidation;

namespace ConsiliumTempus.Application.CustomFieldSetup.Commands.MakeGlobal;

internal sealed class MakeCustomFieldSetupGlobalCommandValidator 
    : AbstractValidator<MakeCustomFieldSetupGlobalCommand>
{
    public MakeCustomFieldSetupGlobalCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}
using FluentValidation;

namespace ConsiliumTempus.Application.CustomFieldSetup.Commands.MakeGlobal;

public sealed class MakeCustomFieldSetupGlobalCommandValidator 
    : AbstractValidator<MakeCustomFieldSetupGlobalCommand>
{
    public MakeCustomFieldSetupGlobalCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}
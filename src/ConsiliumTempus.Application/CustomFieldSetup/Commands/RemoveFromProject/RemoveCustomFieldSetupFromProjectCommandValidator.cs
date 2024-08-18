using FluentValidation;

namespace ConsiliumTempus.Application.CustomFieldSetup.Commands.RemoveFromProject;

public sealed class RemoveCustomFieldSetupFromProjectCommandValidator 
    : AbstractValidator<RemoveCustomFieldSetupFromProjectCommand>
{
    public RemoveCustomFieldSetupFromProjectCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.ProjectId)
            .NotEmpty();
    }
}
using FluentValidation;

namespace ConsiliumTempus.Application.CustomFieldSetup.Commands.AddToProject;

public sealed class AddCustomFieldSetupToProjectCommandValidator 
    : AbstractValidator<AddCustomFieldSetupToProjectCommand>
{
    public AddCustomFieldSetupToProjectCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.ProjectId)
            .NotEmpty();
    }
}
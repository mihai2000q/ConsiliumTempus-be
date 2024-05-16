using ConsiliumTempus.Domain.Common.Validation;
using FluentValidation;

namespace ConsiliumTempus.Application.ProjectSprint.Commands.Update;

public sealed class UpdateProjectSprintCommandValidator : AbstractValidator<UpdateProjectSprintCommand>
{
    public UpdateProjectSprintCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(PropertiesValidation.ProjectSprint.NameMaximumLength);
    }
}
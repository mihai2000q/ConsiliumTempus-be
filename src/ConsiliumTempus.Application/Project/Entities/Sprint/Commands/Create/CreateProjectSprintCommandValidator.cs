using ConsiliumTempus.Domain.Common.Validation;
using FluentValidation;

namespace ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Create;

public sealed class CreateProjectSprintCommandValidator : AbstractValidator<CreateProjectSprintCommand>
{
    public CreateProjectSprintCommandValidator()
    {
        RuleFor(c => c.ProjectId)
            .NotEmpty();

        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(PropertiesValidation.ProjectSprint.NameMaximumLength);
    }
}
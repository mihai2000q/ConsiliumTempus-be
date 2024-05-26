using ConsiliumTempus.Domain.Common.Validation;
using FluentValidation;

namespace ConsiliumTempus.Application.ProjectSprint.Commands.Create;

public sealed class CreateProjectSprintCommandValidator : AbstractValidator<CreateProjectSprintCommand>
{
    public CreateProjectSprintCommandValidator()
    {
        RuleFor(c => c.ProjectId)
            .NotEmpty();

        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(PropertiesValidation.ProjectSprint.NameMaximumLength);

        RuleFor(c => c)
            .Must(c => c.StartDate is null || c.EndDate is null || c.StartDate < c.EndDate)
            .WithMessage("The 'EndDate' must be bigger than the 'StartDate'");
    }
}
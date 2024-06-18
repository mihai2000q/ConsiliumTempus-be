using ConsiliumTempus.Application.Common.Extensions;
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

        RuleFor(c => c)
            .Must(c => c.StartDate < c.EndDate)
            .WithMessage("The 'EndDate' must be bigger than the 'StartDate'")
            .WithName(nameof(UpdateProjectSprintCommand.StartDate).And(nameof(UpdateProjectSprintCommand.EndDate)))
            .When(c => c.StartDate is not null && c.EndDate is not null);
    }
}
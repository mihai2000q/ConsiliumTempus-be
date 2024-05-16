using ConsiliumTempus.Domain.Common.Validation;
using FluentValidation;

namespace ConsiliumTempus.Application.ProjectSprint.Commands.AddStage;

public sealed class AddStageToProjectSprintCommandValidator : AbstractValidator<AddStageToProjectSprintCommand>
{
    public AddStageToProjectSprintCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(PropertiesValidation.ProjectStage.NameMaximumLength);
    }
}
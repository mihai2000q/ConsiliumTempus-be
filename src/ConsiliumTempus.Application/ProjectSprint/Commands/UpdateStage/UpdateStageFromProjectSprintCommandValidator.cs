using ConsiliumTempus.Domain.Common.Validation;
using FluentValidation;

namespace ConsiliumTempus.Application.ProjectSprint.Commands.UpdateStage;

public sealed class UpdateStageFromProjectSprintCommandValidator : AbstractValidator<UpdateStageFromProjectSprintCommand>
{
    public UpdateStageFromProjectSprintCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.StageId)
            .NotEmpty();

        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(PropertiesValidation.ProjectStage.NameMaximumLength);
    }
}
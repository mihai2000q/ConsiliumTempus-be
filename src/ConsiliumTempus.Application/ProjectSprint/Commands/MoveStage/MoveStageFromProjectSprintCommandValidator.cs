using FluentValidation;

namespace ConsiliumTempus.Application.ProjectSprint.Commands.MoveStage;

public sealed class MoveStageFromProjectSprintCommandValidator : AbstractValidator<MoveStageFromProjectSprintCommand>
{
    public MoveStageFromProjectSprintCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.StageId)
            .NotEmpty();

        RuleFor(c => c.OverStageId)
            .NotEmpty();
    }
}
using FluentValidation;

namespace ConsiliumTempus.Application.ProjectSprint.Commands.RemoveStage;

internal sealed class RemoveStageFromProjectSprintCommandValidator : AbstractValidator<RemoveStageFromProjectSprintCommand>
{
    public RemoveStageFromProjectSprintCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.StageId)
            .NotEmpty();
    }
}
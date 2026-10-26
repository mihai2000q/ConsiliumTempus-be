using FluentValidation;

namespace ConsiliumTempus.Application.ProjectTask.Commands.Move;

internal sealed class MoveProjectTaskCommandValidator : AbstractValidator<MoveProjectTaskCommand>
{
    public MoveProjectTaskCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.OverId)
            .NotEmpty();
    }
}
using FluentValidation;

namespace ConsiliumTempus.Application.ProjectTask.Commands.Move;

public sealed class MoveProjectTaskCommandValidator : AbstractValidator<MoveProjectTaskCommand>
{
    public MoveProjectTaskCommandValidator()
    {
        RuleFor(c => c.SprintId)
            .NotEmpty(); 

        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.OverId)
            .NotEmpty();
    }
}
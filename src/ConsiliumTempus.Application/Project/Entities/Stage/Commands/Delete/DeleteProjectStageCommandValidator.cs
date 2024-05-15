using FluentValidation;

namespace ConsiliumTempus.Application.Project.Entities.Stage.Commands.Delete;

public sealed class DeleteProjectStageCommandValidator : AbstractValidator<DeleteProjectStageCommand>
{
    public DeleteProjectStageCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}
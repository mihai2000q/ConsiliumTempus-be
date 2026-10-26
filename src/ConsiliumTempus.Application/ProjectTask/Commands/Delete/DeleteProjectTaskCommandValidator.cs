using FluentValidation;

namespace ConsiliumTempus.Application.ProjectTask.Commands.Delete;

internal sealed class DeleteProjectTaskCommandValidator : AbstractValidator<DeleteProjectTaskCommand>
{
    public DeleteProjectTaskCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.StageId)
            .NotEmpty();
    }
}
using FluentValidation;

namespace ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Delete;

public sealed class DeleteProjectSprintCommandValidator : AbstractValidator<DeleteProjectSprintCommand>
{
    public DeleteProjectSprintCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}
using FluentValidation;

namespace ConsiliumTempus.Application.ProjectSprint.Commands.Delete;

public sealed class DeleteProjectSprintCommandValidator : AbstractValidator<DeleteProjectSprintCommand>
{
    public DeleteProjectSprintCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}
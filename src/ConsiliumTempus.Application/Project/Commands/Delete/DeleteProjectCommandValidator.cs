using FluentValidation;

namespace ConsiliumTempus.Application.Project.Commands.Delete;

internal sealed class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommand>
{
    public DeleteProjectCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}
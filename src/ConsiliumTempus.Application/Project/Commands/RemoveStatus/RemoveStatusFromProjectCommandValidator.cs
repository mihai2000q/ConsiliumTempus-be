using FluentValidation;

namespace ConsiliumTempus.Application.Project.Commands.RemoveStatus;

public sealed class RemoveStatusFromProjectCommandValidator : AbstractValidator<RemoveStatusFromProjectCommand>
{
    public RemoveStatusFromProjectCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.StatusId)
            .NotEmpty();
    }
}
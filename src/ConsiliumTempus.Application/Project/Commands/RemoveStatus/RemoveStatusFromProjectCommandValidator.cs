using FluentValidation;

namespace ConsiliumTempus.Application.Project.Commands.RemoveStatus;

internal sealed class RemoveStatusFromProjectCommandValidator : AbstractValidator<RemoveStatusFromProjectCommand>
{
    public RemoveStatusFromProjectCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.StatusId)
            .NotEmpty();
    }
}
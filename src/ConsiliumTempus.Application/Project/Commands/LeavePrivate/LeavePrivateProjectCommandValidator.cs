using FluentValidation;

namespace ConsiliumTempus.Application.Project.Commands.LeavePrivate;

public sealed class LeavePrivateProjectCommandValidator : AbstractValidator<LeavePrivateProjectCommand>
{
    public LeavePrivateProjectCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}
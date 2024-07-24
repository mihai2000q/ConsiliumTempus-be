using FluentValidation;

namespace ConsiliumTempus.Application.Workspace.Commands.Leave;

public sealed class LeaveWorkspaceCommandValidator : AbstractValidator<LeaveWorkspaceCommand>
{
    public LeaveWorkspaceCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}
using FluentValidation;

namespace ConsiliumTempus.Application.Workspace.Commands.AcceptInvitation;

public sealed class AcceptInvitationToWorkspaceCommandValidator : AbstractValidator<AcceptInvitationToWorkspaceCommand>
{
    public AcceptInvitationToWorkspaceCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.InvitationId)
            .NotEmpty();
    }
}
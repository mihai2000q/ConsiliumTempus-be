using FluentValidation;

namespace ConsiliumTempus.Application.Workspace.Commands.AcceptInvitation;

internal sealed class AcceptInvitationToWorkspaceCommandValidator : AbstractValidator<AcceptInvitationToWorkspaceCommand>
{
    public AcceptInvitationToWorkspaceCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.InvitationId)
            .NotEmpty();
    }
}
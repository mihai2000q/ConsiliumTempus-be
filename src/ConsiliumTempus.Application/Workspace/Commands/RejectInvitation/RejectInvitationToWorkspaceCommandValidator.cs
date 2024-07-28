using FluentValidation;

namespace ConsiliumTempus.Application.Workspace.Commands.RejectInvitation;

public sealed class RejectInvitationToWorkspaceCommandValidator : AbstractValidator<RejectInvitationToWorkspaceCommand>
{
    public RejectInvitationToWorkspaceCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.InvitationId)
            .NotEmpty();
    }
}
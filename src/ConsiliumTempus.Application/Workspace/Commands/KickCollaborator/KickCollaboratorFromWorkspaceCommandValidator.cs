using FluentValidation;

namespace ConsiliumTempus.Application.Workspace.Commands.KickCollaborator;

internal sealed class KickCollaboratorFromWorkspaceCommandValidator
    : AbstractValidator<KickCollaboratorFromWorkspaceCommand>
{
    public KickCollaboratorFromWorkspaceCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.CollaboratorId)
            .NotEmpty();
    }
}
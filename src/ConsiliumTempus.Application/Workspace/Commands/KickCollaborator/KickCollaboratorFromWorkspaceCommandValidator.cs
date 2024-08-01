using FluentValidation;

namespace ConsiliumTempus.Application.Workspace.Commands.KickCollaborator;

public sealed class KickCollaboratorFromWorkspaceCommandValidator 
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
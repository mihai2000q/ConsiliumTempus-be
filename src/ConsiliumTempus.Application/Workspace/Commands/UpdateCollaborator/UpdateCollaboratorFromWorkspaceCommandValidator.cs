using ConsiliumTempus.Application.Common.Extensions;
using FluentValidation;

namespace ConsiliumTempus.Application.Workspace.Commands.UpdateCollaborator;

public sealed class UpdateCollaboratorFromWorkspaceCommandValidator 
    : AbstractValidator<UpdateCollaboratorFromWorkspaceCommand>
{
    public UpdateCollaboratorFromWorkspaceCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.CollaboratorId)
            .NotEmpty();

        RuleFor(c => c.WorkspaceRole)
            .IsWorkspaceRole();
    }
}
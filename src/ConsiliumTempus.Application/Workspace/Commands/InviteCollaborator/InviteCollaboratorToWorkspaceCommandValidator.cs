using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Validation;
using FluentValidation;

namespace ConsiliumTempus.Application.Workspace.Commands.InviteCollaborator;

public sealed class InviteCollaboratorToWorkspaceCommandValidator 
    : AbstractValidator<InviteCollaboratorToWorkspaceCommand>
{
    public InviteCollaboratorToWorkspaceCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.Email)
            .NotEmpty()
            .MaximumLength(PropertiesValidation.User.EmailMaximumLength)
            .IsEmail();
    }
}
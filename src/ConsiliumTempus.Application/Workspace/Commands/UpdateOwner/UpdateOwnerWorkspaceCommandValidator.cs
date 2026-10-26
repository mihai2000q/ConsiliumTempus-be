using FluentValidation;

namespace ConsiliumTempus.Application.Workspace.Commands.UpdateOwner;

internal sealed class UpdateOwnerWorkspaceCommandValidator : AbstractValidator<UpdateOwnerWorkspaceCommand>
{
    public UpdateOwnerWorkspaceCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.OwnerId)
            .NotEmpty();
    }
}
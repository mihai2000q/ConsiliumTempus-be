using FluentValidation;

namespace ConsiliumTempus.Application.Workspace.Commands.Delete;

internal sealed class DeleteWorkspaceCommandValidator : AbstractValidator<DeleteWorkspaceCommand>
{
    public DeleteWorkspaceCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}
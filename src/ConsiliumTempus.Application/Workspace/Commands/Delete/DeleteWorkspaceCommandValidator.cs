using FluentValidation;

namespace ConsiliumTempus.Application.Workspace.Commands.Delete;

public sealed class DeleteWorkspaceCommandValidator : AbstractValidator<DeleteWorkspaceCommand>
{
    public DeleteWorkspaceCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}
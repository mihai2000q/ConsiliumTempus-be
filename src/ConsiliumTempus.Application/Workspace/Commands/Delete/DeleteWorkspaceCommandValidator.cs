using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace ConsiliumTempus.Application.Workspace.Commands.Delete;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public sealed class DeleteWorkspaceCommandValidator : AbstractValidator<DeleteWorkspaceCommand>
{
    public DeleteWorkspaceCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}
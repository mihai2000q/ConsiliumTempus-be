using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Delete;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public sealed class DeleteProjectSprintCommandValidator : AbstractValidator<DeleteProjectSprintCommand>
{
    public DeleteProjectSprintCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}
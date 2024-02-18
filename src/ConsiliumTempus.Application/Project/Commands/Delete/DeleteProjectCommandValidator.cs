using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace ConsiliumTempus.Application.Project.Commands.Delete;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public sealed class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommand>
{
    public DeleteProjectCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}
using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Validation;
using FluentValidation;

namespace ConsiliumTempus.Application.Workspace.Commands.Create;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public sealed class CreateWorkspaceCommandValidator : AbstractValidator<CreateWorkspaceCommand>
{
    public CreateWorkspaceCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(PropertiesValidation.Workspace.NameMaximumLength);
        
        RuleFor(c => c.Description)
            .NotEmpty()
            .MaximumLength(PropertiesValidation.Workspace.DescriptionMaximumLength);
    }
}
using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Validation;
using FluentValidation;

namespace ConsiliumTempus.Application.Workspace.Commands.Update;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public sealed class UpdateWorkspaceCommandValidator : AbstractValidator<UpdateWorkspaceCommand>
{
    public UpdateWorkspaceCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
        
        RuleFor(c => c.Name)
            .MaximumLength(PropertiesValidation.Workspace.NameMaximumLength);
        
        RuleFor(c => c.Description)
            .MaximumLength(PropertiesValidation.Workspace.DescriptionMaximumLength);
    }
}
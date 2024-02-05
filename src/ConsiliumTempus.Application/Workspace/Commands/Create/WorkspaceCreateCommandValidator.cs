using ConsiliumTempus.Domain.Common.Validation;
using FluentValidation;

namespace ConsiliumTempus.Application.Workspace.Commands.Create;

public class WorkspaceCreateCommandValidator : AbstractValidator<WorkspaceCreateCommand>
{
    public WorkspaceCreateCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(PropertiesValidation.Workspace.NameMaximumLength);
        
        RuleFor(c => c.Description)
            .NotEmpty()
            .MaximumLength(PropertiesValidation.Workspace.DescriptionMaximumLength);
    }
}
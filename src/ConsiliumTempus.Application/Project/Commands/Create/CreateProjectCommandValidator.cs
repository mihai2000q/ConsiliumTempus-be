using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Validation;
using FluentValidation;

namespace ConsiliumTempus.Application.Project.Commands.Create;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public sealed class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(PropertiesValidation.Project.NameMaximumLength);
        
        RuleFor(c => c.Description)
            .NotEmpty()
            .MaximumLength(PropertiesValidation.Project.DescriptionMaximumLength);

        RuleFor(c => c.IsPrivate)
            .NotEmpty();
    }
}
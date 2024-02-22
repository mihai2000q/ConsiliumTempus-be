using ConsiliumTempus.Domain.Common.Validation;
using FluentValidation;

namespace ConsiliumTempus.Application.Project.Commands.Create;

public sealed class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(c => c.WorkspaceId)
            .NotEmpty();

        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(PropertiesValidation.Project.NameMaximumLength);

        RuleFor(c => c.Description)
            .NotNull()
            .MaximumLength(PropertiesValidation.Project.DescriptionMaximumLength);

        RuleFor(c => c.IsPrivate)
            .NotEmpty();
    }
}
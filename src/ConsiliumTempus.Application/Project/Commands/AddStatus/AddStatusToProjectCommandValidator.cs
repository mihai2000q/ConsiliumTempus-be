using ConsiliumTempus.Domain.Common.Validation;
using FluentValidation;

namespace ConsiliumTempus.Application.Project.Commands.AddStatus;

public sealed class AddStatusToProjectCommandValidator : AbstractValidator<AddStatusToProjectCommand>
{
    public AddStatusToProjectCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.Title)
            .NotEmpty()
            .MaximumLength(PropertiesValidation.ProjectStatus.TitleMaximumLength);

        RuleFor(c => c.Description)
            .NotEmpty();
    }
}
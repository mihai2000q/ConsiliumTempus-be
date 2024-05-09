using ConsiliumTempus.Domain.Common.Validation;
using FluentValidation;

namespace ConsiliumTempus.Application.Project.Entities.Stage.Commands.Create;

public sealed class CreateProjectStageCommandValidator : AbstractValidator<CreateProjectStageCommand>
{
    public CreateProjectStageCommandValidator()
    {
        RuleFor(c => c.ProjectSprintId)
            .NotEmpty();

        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(PropertiesValidation.ProjectStage.NameMaximumLength);
    }
}
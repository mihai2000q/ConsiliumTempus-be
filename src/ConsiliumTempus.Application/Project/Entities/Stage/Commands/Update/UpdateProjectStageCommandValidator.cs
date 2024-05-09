using ConsiliumTempus.Domain.Common.Validation;
using FluentValidation;

namespace ConsiliumTempus.Application.Project.Entities.Stage.Commands.Update;

public sealed class UpdateProjectStageCommandValidator : AbstractValidator<UpdateProjectStageCommand>
{
    public UpdateProjectStageCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(PropertiesValidation.ProjectStage.NameMaximumLength);
    }
}
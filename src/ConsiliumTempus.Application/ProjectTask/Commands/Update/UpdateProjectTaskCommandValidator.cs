using ConsiliumTempus.Domain.Common.Validation;
using FluentValidation;

namespace ConsiliumTempus.Application.ProjectTask.Commands.Update;

public sealed class UpdateProjectTaskCommandValidator : AbstractValidator<UpdateProjectTaskCommand>
{
    public UpdateProjectTaskCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(PropertiesValidation.ProjectTask.NameMaximumLength);
    }
}
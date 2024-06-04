using ConsiliumTempus.Domain.Common.Validation;
using ConsiliumTempus.Domain.Project.Enums;
using FluentValidation;

namespace ConsiliumTempus.Application.Project.Commands.UpdateStatus;

public sealed class UpdateStatusFromProjectCommandValidator : AbstractValidator<UpdateStatusFromProjectCommand>
{
    public UpdateStatusFromProjectCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.StatusId)
            .NotEmpty();

        RuleFor(c => c.Title)
            .NotEmpty()
            .MaximumLength(PropertiesValidation.ProjectStatus.TitleMaximumLength);

        RuleFor(c => c.Status)
            .NotEmpty()
            .IsEnumName(typeof(ProjectStatusType), false);

        RuleFor(c => c.Description)
            .NotEmpty();
    }
}
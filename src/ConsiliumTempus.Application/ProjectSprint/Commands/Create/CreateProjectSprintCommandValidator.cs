using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Validation;
using ConsiliumTempus.Domain.Project.Enums;
using FluentValidation;

namespace ConsiliumTempus.Application.ProjectSprint.Commands.Create;

public sealed class CreateProjectSprintCommandValidator : AbstractValidator<CreateProjectSprintCommand>
{
    public CreateProjectSprintCommandValidator()
    {
        RuleFor(c => c.ProjectId)
            .NotEmpty();

        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(PropertiesValidation.ProjectSprint.NameMaximumLength);

        RuleFor(c => c)
            .Must(c => c.StartDate < c.EndDate)
            .WithMessage("The 'EndDate' must be bigger than the 'StartDate'")
            .WithName(nameof(CreateProjectSprintCommand.StartDate).And(nameof(CreateProjectSprintCommand.EndDate)))
            .When(c => c.StartDate is not null && c.EndDate is not null);

        When(c => c.ProjectStatus is not null, () =>
        {
            RuleFor(c => c.ProjectStatus!.Title)
                .NotEmpty()
                .MaximumLength(PropertiesValidation.ProjectStatus.TitleMaximumLength);

            RuleFor(c => c.ProjectStatus!.Status)
                .NotEmpty()
                .IsEnumName(typeof(ProjectStatusType), false);

            RuleFor(c => c.ProjectStatus!.Description)
                .NotEmpty();
        });
    }
}
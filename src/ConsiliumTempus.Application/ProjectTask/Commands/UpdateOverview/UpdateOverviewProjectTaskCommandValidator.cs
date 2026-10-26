using ConsiliumTempus.Domain.Common.Validation;
using FluentValidation;

namespace ConsiliumTempus.Application.ProjectTask.Commands.UpdateOverview;

internal sealed class UpdateOverviewProjectTaskCommandValidator : AbstractValidator<UpdateOverviewProjectTaskCommand>
{
    public UpdateOverviewProjectTaskCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(PropertiesValidation.ProjectTask.NameMaximumLength);
    }
}
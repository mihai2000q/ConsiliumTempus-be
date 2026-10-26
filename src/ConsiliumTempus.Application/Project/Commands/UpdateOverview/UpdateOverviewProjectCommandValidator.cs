using FluentValidation;

namespace ConsiliumTempus.Application.Project.Commands.UpdateOverview;

internal sealed class UpdateOverviewProjectCommandValidator : AbstractValidator<UpdateOverviewProjectCommand>
{
    public UpdateOverviewProjectCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}
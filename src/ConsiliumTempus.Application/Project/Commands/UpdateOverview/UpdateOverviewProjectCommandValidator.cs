using FluentValidation;

namespace ConsiliumTempus.Application.Project.Commands.UpdateOverview;

public sealed class UpdateOverviewProjectCommandValidator : AbstractValidator<UpdateOverviewProjectCommand>
{
    public UpdateOverviewProjectCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}
using FluentValidation;

namespace ConsiliumTempus.Application.Workspace.Commands.UpdateOverview;

public sealed class UpdateOverviewWorkspaceCommandValidator : AbstractValidator<UpdateOverviewWorkspaceCommand>
{
    public UpdateOverviewWorkspaceCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}
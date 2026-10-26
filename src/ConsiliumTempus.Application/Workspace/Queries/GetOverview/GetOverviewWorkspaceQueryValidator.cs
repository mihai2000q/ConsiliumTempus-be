using FluentValidation;

namespace ConsiliumTempus.Application.Workspace.Queries.GetOverview;

internal sealed class GetOverviewWorkspaceQueryValidator : AbstractValidator<GetOverviewWorkspaceQuery>
{
    public GetOverviewWorkspaceQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();
    }
}
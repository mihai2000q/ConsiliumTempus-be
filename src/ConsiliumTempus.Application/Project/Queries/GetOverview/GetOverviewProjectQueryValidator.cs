using FluentValidation;

namespace ConsiliumTempus.Application.Project.Queries.GetOverview;

public sealed class GetOverviewProjectQueryValidator : AbstractValidator<GetOverviewProjectQuery>
{
    public GetOverviewProjectQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();
    }
}
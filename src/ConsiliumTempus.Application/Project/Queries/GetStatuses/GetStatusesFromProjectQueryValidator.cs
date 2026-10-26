using FluentValidation;

namespace ConsiliumTempus.Application.Project.Queries.GetStatuses;

internal sealed class GetStatusesFromProjectQueryValidator : AbstractValidator<GetStatusesFromProjectQuery>
{
    public GetStatusesFromProjectQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();
    }
}
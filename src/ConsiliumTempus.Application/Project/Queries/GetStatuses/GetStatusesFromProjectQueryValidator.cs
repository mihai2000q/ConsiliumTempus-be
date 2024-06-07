using FluentValidation;

namespace ConsiliumTempus.Application.Project.Queries.GetStatuses;

public sealed class GetStatusesFromProjectQueryValidator : AbstractValidator<GetStatusesFromProjectQuery>
{
    public GetStatusesFromProjectQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();
    }
}
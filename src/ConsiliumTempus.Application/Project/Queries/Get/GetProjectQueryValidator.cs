using FluentValidation;

namespace ConsiliumTempus.Application.Project.Queries.Get;

public sealed class GetProjectQueryValidator : AbstractValidator<GetProjectQuery>
{
    public GetProjectQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();
    }
}
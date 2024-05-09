using FluentValidation;

namespace ConsiliumTempus.Application.Project.Entities.Sprint.Queries.Get;

public sealed class GetProjectSprintQueryValidator : AbstractValidator<GetProjectSprintQuery>
{
    public GetProjectSprintQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();
    }
}
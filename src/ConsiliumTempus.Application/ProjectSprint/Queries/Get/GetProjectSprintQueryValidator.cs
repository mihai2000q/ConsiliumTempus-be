using FluentValidation;

namespace ConsiliumTempus.Application.ProjectSprint.Queries.Get;

public sealed class GetProjectSprintQueryValidator : AbstractValidator<GetProjectSprintQuery>
{
    public GetProjectSprintQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();
    }
}
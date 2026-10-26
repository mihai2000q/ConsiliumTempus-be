using FluentValidation;

namespace ConsiliumTempus.Application.ProjectSprint.Queries.Get;

internal sealed class GetProjectSprintQueryValidator : AbstractValidator<GetProjectSprintQuery>
{
    public GetProjectSprintQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();
    }
}
using FluentValidation;

namespace ConsiliumTempus.Application.ProjectSprint.Queries.GetStages;

public sealed class GetStagesFromProjectSprintQueryValidator : AbstractValidator<GetStagesFromProjectSprintQuery>
{
    public GetStagesFromProjectSprintQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();
    }
}
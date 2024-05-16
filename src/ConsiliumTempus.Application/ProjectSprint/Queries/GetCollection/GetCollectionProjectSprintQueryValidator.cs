using FluentValidation;

namespace ConsiliumTempus.Application.ProjectSprint.Queries.GetCollection;

public sealed class GetCollectionProjectSprintQueryValidator : AbstractValidator<GetCollectionProjectSprintQuery>
{
    public GetCollectionProjectSprintQueryValidator()
    {
        RuleFor(q => q.ProjectId)
            .NotEmpty();
    }
}
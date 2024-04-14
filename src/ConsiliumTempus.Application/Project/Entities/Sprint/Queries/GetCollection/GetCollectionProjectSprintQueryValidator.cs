using FluentValidation;

namespace ConsiliumTempus.Application.Project.Entities.Sprint.Queries.GetCollection;

public sealed class GetCollectionProjectSprintQueryValidator : AbstractValidator<GetCollectionProjectSprintQuery>
{
    public GetCollectionProjectSprintQueryValidator()
    {
        RuleFor(q => q.ProjectId)
            .NotEmpty();
    }
}
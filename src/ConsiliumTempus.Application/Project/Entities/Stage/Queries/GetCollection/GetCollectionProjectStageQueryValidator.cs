using FluentValidation;

namespace ConsiliumTempus.Application.Project.Entities.Stage.Queries.GetCollection;

public sealed class GetCollectionProjectStageQueryValidator : AbstractValidator<GetCollectionProjectStageQuery>
{
    public GetCollectionProjectStageQueryValidator()
    {
        RuleFor(q => q.ProjectSprintId)
            .NotEmpty();
    }
}
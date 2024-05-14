using FluentValidation;

namespace ConsiliumTempus.Application.ProjectTask.Queries.GetCollection;

public class GetCollectionProjectTaskQueryValidator : AbstractValidator<GetCollectionProjectTaskQuery>
{
    public GetCollectionProjectTaskQueryValidator()
    {
        RuleFor(q => q.ProjectStageId)
            .NotEmpty();
    }
}
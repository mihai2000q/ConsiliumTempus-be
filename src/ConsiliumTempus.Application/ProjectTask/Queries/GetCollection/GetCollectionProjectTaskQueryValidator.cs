using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Filters;
using FluentValidation;

namespace ConsiliumTempus.Application.ProjectTask.Queries.GetCollection;

public class GetCollectionProjectTaskQueryValidator : AbstractValidator<GetCollectionProjectTaskQuery>
{
    public GetCollectionProjectTaskQueryValidator()
    {
        RuleFor(q => q.ProjectStageId)
            .NotEmpty();

        RuleFor(q => q.Search)
            .HasSearchFormat(ProjectTaskFilter.FilterProperties);
    }
}
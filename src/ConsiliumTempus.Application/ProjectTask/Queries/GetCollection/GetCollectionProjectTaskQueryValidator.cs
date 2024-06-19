using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Filters;
using ConsiliumTempus.Domain.Common.Orders;
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

        RuleFor(q => q.OrderBy)
            .HasOrderByFormat(ProjectTaskOrder.OrderProperties);
    }
}
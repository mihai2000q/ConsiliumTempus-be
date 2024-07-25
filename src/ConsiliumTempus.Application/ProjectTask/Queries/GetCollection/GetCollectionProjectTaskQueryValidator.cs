using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Filters;
using ConsiliumTempus.Domain.Common.Orders;
using FluentValidation;

namespace ConsiliumTempus.Application.ProjectTask.Queries.GetCollection;

public sealed class GetCollectionProjectTaskQueryValidator : AbstractValidator<GetCollectionProjectTaskQuery>
{
    public GetCollectionProjectTaskQueryValidator()
    {
        RuleFor(q => q.ProjectStageId)
            .NotEmpty();

        RuleFor(q => q.Search)
            .HasSearchFormat(ProjectTaskFilter.FilterProperties);

        RuleFor(q => q.OrderBy)
            .HasOrderByFormat(ProjectTaskOrder.OrderProperties);
        
        RuleFor(q => q)
            .Must(q => q.PageSize is not null ? q.CurrentPage is not null : q.CurrentPage is null)
            .WithMessage("Both the 'PageSize' and the 'CurrentPage' have to either be set or unset.")
            .WithName(nameof(GetCollectionProjectTaskQuery.PageSize).And(nameof(GetCollectionProjectTaskQuery.CurrentPage)));

        RuleFor(q => q.PageSize)
            .GreaterThan(0);

        RuleFor(q => q.CurrentPage)
            .GreaterThan(0);
    }
}
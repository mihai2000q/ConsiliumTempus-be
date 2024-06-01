using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Filters;
using ConsiliumTempus.Domain.Common.Orders;
using FluentValidation;

namespace ConsiliumTempus.Application.Project.Queries.GetCollection;

public sealed class GetCollectionProjectQueryValidator : AbstractValidator<GetCollectionProjectQuery>
{
    public GetCollectionProjectQueryValidator()
    {
        RuleFor(q => q.PageSize)
            .GreaterThan(0);

        RuleFor(q => q.CurrentPage)
            .GreaterThan(0);

        RuleFor(q => q.OrderBy)
            .HasOrderByFormat(ProjectOrder.OrderProperties);

        RuleFor(q => q.Search)
            .HasSearchFormat(ProjectFilter.FilterProperties);
    }
}
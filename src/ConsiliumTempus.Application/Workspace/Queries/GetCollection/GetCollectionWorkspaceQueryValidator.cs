using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Filters;
using ConsiliumTempus.Domain.Common.Orders;
using FluentValidation;

namespace ConsiliumTempus.Application.Workspace.Queries.GetCollection;

public sealed class GetCollectionWorkspaceQueryValidator : AbstractValidator<GetCollectionWorkspaceQuery>
{
    public GetCollectionWorkspaceQueryValidator()
    {
        RuleFor(q => q.PageSize)
            .GreaterThan(0);

        RuleFor(q => q.CurrentPage)
            .GreaterThan(0);

        RuleFor(q => q.OrderBy)
            .HasOrderByFormat(WorkspaceOrder.OrderProperties);

        RuleFor(q => q.Search)
            .HasSearchFormat(WorkspaceFilter.FilterProperties);
    }
}
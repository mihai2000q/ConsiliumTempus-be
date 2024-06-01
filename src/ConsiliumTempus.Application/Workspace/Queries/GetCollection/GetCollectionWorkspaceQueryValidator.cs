using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Orders;
using ConsiliumTempus.Domain.Common.Validation;
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

        RuleFor(q => q.Name)
            .MaximumLength(PropertiesValidation.Workspace.NameMaximumLength);
    }
}
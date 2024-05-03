using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Orders;
using FluentValidation;

namespace ConsiliumTempus.Application.Workspace.Queries.GetCollection;

public sealed class GetCollectionWorkspaceQueryValidator : AbstractValidator<GetCollectionWorkspaceQuery>
{
    public GetCollectionWorkspaceQueryValidator()
    {
        RuleFor(q => q.Order)
            .HasOrderFormat(WorkspaceOrder.OrderProperties);
    }
}
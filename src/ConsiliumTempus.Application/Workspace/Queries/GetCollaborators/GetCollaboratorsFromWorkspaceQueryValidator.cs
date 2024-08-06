using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Filters;
using ConsiliumTempus.Domain.Common.Orders;
using FluentValidation;

namespace ConsiliumTempus.Application.Workspace.Queries.GetCollaborators;

public sealed class GetCollaboratorsFromWorkspaceQueryValidator : AbstractValidator<GetCollaboratorsFromWorkspaceQuery>
{
    public GetCollaboratorsFromWorkspaceQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();

        RuleFor(q => q)
            .Must(q => q.PageSize is not null ? q.CurrentPage is not null : q.CurrentPage is null)
            .WithMessage("Both the 'PageSize' and the 'CurrentPage' have to either be set or unset.")
            .WithName(nameof(GetCollaboratorsFromWorkspaceQuery.PageSize)
                .And(nameof(GetCollaboratorsFromWorkspaceQuery.CurrentPage)));

        RuleFor(q => q.PageSize)
            .GreaterThan(0);

        RuleFor(q => q.CurrentPage)
            .GreaterThan(0);

        RuleFor(q => q.OrderBy)
            .HasOrderByFormat(MembershipOrder.OrderProperties);

        RuleFor(q => q.Search)
            .HasSearchFormat(MembershipFilter.FilterProperties);
    }
}
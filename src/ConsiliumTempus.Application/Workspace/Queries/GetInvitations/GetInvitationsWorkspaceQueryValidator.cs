using ConsiliumTempus.Application.Common.Extensions;
using FluentValidation;

namespace ConsiliumTempus.Application.Workspace.Queries.GetInvitations;

public sealed class GetInvitationsWorkspaceQueryValidator : AbstractValidator<GetInvitationsWorkspaceQuery>
{
    public GetInvitationsWorkspaceQueryValidator()
    {
        RuleFor(q => q)
            .Must(q => q.IsSender is null ? q.WorkspaceId is not null : q.WorkspaceId is null)
            .WithMessage("Exactly one of the 'IsSender' or 'WorkspaceId' parameters must be set.")
            .WithName(nameof(GetInvitationsWorkspaceQuery.IsSender).And(nameof(GetInvitationsWorkspaceQuery.WorkspaceId)));

        RuleFor(q => q)
            .Must(q => q.PageSize is not null ? q.CurrentPage is not null : q.CurrentPage is null)
            .WithMessage("Both the 'PageSize' and the 'CurrentPage' have to either be set or unset.")
            .WithName(nameof(GetInvitationsWorkspaceQuery.PageSize).And(nameof(GetInvitationsWorkspaceQuery.CurrentPage)));

        RuleFor(q => q.PageSize)
            .GreaterThan(0);

        RuleFor(q => q.CurrentPage)
            .GreaterThan(0);
    }
}
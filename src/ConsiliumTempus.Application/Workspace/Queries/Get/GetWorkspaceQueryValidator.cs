using FluentValidation;

namespace ConsiliumTempus.Application.Workspace.Queries.Get;

internal sealed class GetWorkspaceQueryValidator : AbstractValidator<GetWorkspaceQuery>
{
    public GetWorkspaceQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();
    }
}
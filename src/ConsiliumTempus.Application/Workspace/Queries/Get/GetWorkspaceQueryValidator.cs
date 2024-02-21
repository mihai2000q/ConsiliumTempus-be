using FluentValidation;

namespace ConsiliumTempus.Application.Workspace.Queries.Get;

public sealed class GetWorkspaceQueryValidator : AbstractValidator<GetWorkspaceQuery>
{
    public GetWorkspaceQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();
    }
}
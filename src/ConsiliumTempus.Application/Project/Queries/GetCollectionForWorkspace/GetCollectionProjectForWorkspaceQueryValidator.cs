using FluentValidation;

namespace ConsiliumTempus.Application.Project.Queries.GetCollectionForWorkspace;

public sealed class GetCollectionProjectForWorkspaceQueryValidator
    : AbstractValidator<GetCollectionProjectForWorkspaceQuery>
{
    public GetCollectionProjectForWorkspaceQueryValidator()
    {
        RuleFor(q => q.WorkspaceId)
            .NotEmpty();
    }
}
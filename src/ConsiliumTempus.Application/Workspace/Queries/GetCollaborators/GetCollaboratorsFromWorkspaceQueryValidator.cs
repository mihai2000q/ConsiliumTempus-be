using FluentValidation;

namespace ConsiliumTempus.Application.Workspace.Queries.GetCollaborators;

public sealed class GetCollaboratorsFromWorkspaceQueryValidator : AbstractValidator<GetCollaboratorsFromWorkspaceQuery>
{
    public GetCollaboratorsFromWorkspaceQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();
    }
}
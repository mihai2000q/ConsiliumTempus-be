using ConsiliumTempus.Application.Common.Validation;
using FluentValidation;

namespace ConsiliumTempus.Application.Workspace.Queries.Get;

public class GetWorkspaceQueryValidator : AbstractValidator<GetWorkspaceQuery>
{
    public GetWorkspaceQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty()
            .IsId();
    }
}
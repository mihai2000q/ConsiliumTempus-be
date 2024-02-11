using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace ConsiliumTempus.Application.Workspace.Queries.Get;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public sealed class GetWorkspaceQueryValidator : AbstractValidator<GetWorkspaceQuery>
{
    public GetWorkspaceQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();
    }
}
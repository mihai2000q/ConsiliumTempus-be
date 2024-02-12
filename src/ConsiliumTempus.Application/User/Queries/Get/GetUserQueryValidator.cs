using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace ConsiliumTempus.Application.User.Queries.Get;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public sealed class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}
using FluentValidation;

namespace ConsiliumTempus.Application.User.Queries.Get;

internal sealed class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}
using FluentValidation;

namespace ConsiliumTempus.Application.User.Queries.Get;

public sealed class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}
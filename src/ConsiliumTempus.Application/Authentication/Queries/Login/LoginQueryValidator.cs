using ConsiliumTempus.Application.Common.Validation;
using FluentValidation;

namespace ConsiliumTempus.Application.Authentication.Queries.Login;

public class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        RuleFor(q => q.Email)
            .NotEmpty()
            .IsEmail();
        RuleFor(q => q.Password)
            .NotEmpty()
            .IsPassword();
    }
}
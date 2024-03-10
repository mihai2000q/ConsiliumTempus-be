using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Validation;
using FluentValidation;

namespace ConsiliumTempus.Application.Authentication.Queries.Login;

public sealed class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        RuleFor(q => q.Email)
            .NotEmpty()
            .MaximumLength(PropertiesValidation.User.EmailMaximumLength)
            .IsEmail();
        
        RuleFor(q => q.Password)
            .NotEmpty()
            .MaximumLength(PropertiesValidation.User.PlainPasswordMaximumLength)
            .IsPassword();
    }
}
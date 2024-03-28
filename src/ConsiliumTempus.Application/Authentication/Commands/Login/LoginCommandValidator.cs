using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Validation;
using FluentValidation;

namespace ConsiliumTempus.Application.Authentication.Commands.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
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
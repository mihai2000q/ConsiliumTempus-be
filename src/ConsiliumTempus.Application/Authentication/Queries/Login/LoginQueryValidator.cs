using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Application.Common.Validation;
using FluentValidation;

namespace ConsiliumTempus.Application.Authentication.Queries.Login;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public sealed class LoginQueryValidator : AbstractValidator<LoginQuery>
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
using ConsiliumTempus.Application.Common.Validation;
using FluentValidation;

namespace ConsiliumTempus.Application.Authentication.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(c => c.FirstName)
            .NotEmpty();
        RuleFor(c => c.LastName)
            .NotEmpty();
        RuleFor(c => c.Email)
            .NotEmpty()
            .IsEmail();
        RuleFor(c => c.Password)
            .NotEmpty()
            .IsPassword();
    }
}
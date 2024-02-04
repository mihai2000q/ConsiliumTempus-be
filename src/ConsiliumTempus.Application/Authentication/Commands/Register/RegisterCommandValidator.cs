using ConsiliumTempus.Application.Common.Validation;
using ConsiliumTempus.Domain.Common.Validation;
using FluentValidation;

namespace ConsiliumTempus.Application.Authentication.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(c => c.FirstName)
            .NotEmpty()
            .MaximumLength(PropertiesValidation.User.FirstNameMaximumLength);
        RuleFor(c => c.LastName)
            .NotEmpty()
            .MaximumLength(PropertiesValidation.User.LastNameMaximumLength);
        RuleFor(c => c.Email)
            .NotEmpty()
            .MaximumLength(PropertiesValidation.User.EmailMaximumLength)
            .IsEmail();
        RuleFor(c => c.Password)
            .NotEmpty()
            .MaximumLength(PropertiesValidation.User.PlainPasswordMaximumLength)
            .IsPassword();
    }
}
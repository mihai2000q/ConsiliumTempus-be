using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Validation;
using FluentValidation;

namespace ConsiliumTempus.Application.User.Commands.Update;

public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.FirstName)
            .NotEmpty()
            .MaximumLength(PropertiesValidation.User.FirstNameMaximumLength);
        
        RuleFor(c => c.LastName)
            .NotEmpty()
            .MaximumLength(PropertiesValidation.User.LastNameMaximumLength);

        RuleFor(c => c.Role)
            .MaximumLength(PropertiesValidation.User.RoleMaximumLength);
        
        RuleFor(c => c.DateOfBirth)
            .IsPastDate();
    }
}
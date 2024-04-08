using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Validation;
using FluentValidation;

namespace ConsiliumTempus.Application.User.Commands.UpdateCurrent;

public sealed class UpdateCurrentUserCommandValidator : AbstractValidator<UpdateCurrentUserCommand>
{
    public UpdateCurrentUserCommandValidator()
    {
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
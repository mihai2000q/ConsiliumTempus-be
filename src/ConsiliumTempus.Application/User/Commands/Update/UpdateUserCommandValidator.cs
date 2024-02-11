using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Validation;
using FluentValidation;

namespace ConsiliumTempus.Application.User.Commands.Update;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.FirstName)
            .MaximumLength(PropertiesValidation.User.FirstNameMaximumLength);
        
        RuleFor(c => c.LastName)
            .MaximumLength(PropertiesValidation.User.LastNameMaximumLength);
    }
}
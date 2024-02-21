using FluentValidation;

namespace ConsiliumTempus.Application.User.Commands.Delete;

public sealed class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}
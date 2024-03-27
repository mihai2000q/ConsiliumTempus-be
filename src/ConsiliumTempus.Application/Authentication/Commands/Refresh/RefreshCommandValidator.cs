using FluentValidation;

namespace ConsiliumTempus.Application.Authentication.Commands.Refresh;

public sealed class RefreshCommandValidator : AbstractValidator<RefreshCommand>
{
    public RefreshCommandValidator()
    {
        RuleFor(c => c.Token)
            .NotEmpty();

        RuleFor(c => c.RefreshToken)
            .NotEmpty();
    }
}
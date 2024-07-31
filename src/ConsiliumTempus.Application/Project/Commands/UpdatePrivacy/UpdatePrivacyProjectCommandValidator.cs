using FluentValidation;

namespace ConsiliumTempus.Application.Project.Commands.UpdatePrivacy;

public sealed class UpdatePrivacyProjectCommandValidator : AbstractValidator<UpdatePrivacyProjectCommand>
{
    public UpdatePrivacyProjectCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}
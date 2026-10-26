using FluentValidation;

namespace ConsiliumTempus.Application.Project.Commands.UpdatePrivacy;

internal sealed class UpdatePrivacyProjectCommandValidator : AbstractValidator<UpdatePrivacyProjectCommand>
{
    public UpdatePrivacyProjectCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}
using FluentValidation;

namespace ConsiliumTempus.Application.Project.Commands.RemoveAllowedMember;

internal sealed class RemoveAllowedMemberFromProjectCommandValidator
    : AbstractValidator<RemoveAllowedMemberFromProjectCommand>
{
    public RemoveAllowedMemberFromProjectCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.AllowedMemberId)
            .NotEmpty();
    }
}
using FluentValidation;

namespace ConsiliumTempus.Application.Project.Commands.AddAllowedMember;

public sealed class AddAllowedMemberToProjectCommandValidator : AbstractValidator<AddAllowedMemberToProjectCommand>
{
    public AddAllowedMemberToProjectCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.CollaboratorId)
            .NotEmpty();
    }
}
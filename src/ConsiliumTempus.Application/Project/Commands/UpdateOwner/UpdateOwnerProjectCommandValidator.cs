using FluentValidation;

namespace ConsiliumTempus.Application.Project.Commands.UpdateOwner;

public sealed class UpdateOwnerProjectCommandValidator : AbstractValidator<UpdateOwnerProjectCommand>
{
    public UpdateOwnerProjectCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.OwnerId)
            .NotEmpty();
    }
}
using FluentValidation;

namespace ConsiliumTempus.Application.ProjectTask.Commands.UpdateIsCompleted;

internal sealed class UpdateIsCompletedProjectTaskCommandValidator : AbstractValidator<UpdateIsCompletedProjectTaskCommand>
{
    public UpdateIsCompletedProjectTaskCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}
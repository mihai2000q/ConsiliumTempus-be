using FluentValidation;

namespace ConsiliumTempus.Application.CustomFieldSetup.Commands.UpdateWorkspace;

public sealed class UpdateWorkspaceCustomFieldSetupCommandValidator 
    : AbstractValidator<UpdateWorkspaceCustomFieldSetupCommand>
{
    public UpdateWorkspaceCustomFieldSetupCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.WorkspaceId)
            .NotEmpty();
    }
}
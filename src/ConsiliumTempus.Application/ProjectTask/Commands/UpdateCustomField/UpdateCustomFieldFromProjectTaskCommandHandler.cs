using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectTask.Commands.UpdateCustomField;

public sealed class UpdateCustomFieldFromProjectTaskCommandHandler(IProjectTaskRepository projectTaskRepository)
    : IRequestHandler<UpdateCustomFieldFromProjectTaskCommand, ErrorOr<UpdateCustomFieldFromProjectTaskResult>>
{
    public async Task<ErrorOr<UpdateCustomFieldFromProjectTaskResult>> Handle(
        UpdateCustomFieldFromProjectTaskCommand command, 
        CancellationToken cancellationToken)
    {
        var task = await projectTaskRepository.GetWithCustomFieldsAndWorkspace(
            ProjectTaskId.Create(command.Id),
            cancellationToken);
        if (task is null) return Errors.ProjectTask.NotFound;
        
        var customField = task.CustomFields.SingleOrDefault(cf => cf.Id.Value == command.CustomFieldId);
        if (customField is null) return Errors.CustomField.NotFound;

        switch (customField)
        {
            case NumberCustomField numberCustomField:
                UpdateNumberCustomField(numberCustomField, command);
                break;
            case SingleSelectCustomField singleSelectCustomField:
                UpdateSingleSelectCustomField(singleSelectCustomField, command);
                break;
            case TextCustomField textCustomField:
                UpdateTextCustomField(textCustomField, command);
                break;
        }
        task.RefreshUpdatedDateTime();
        task.Stage.Sprint.Project.RefreshActivity();
        
        return new UpdateCustomFieldFromProjectTaskResult();
    }

    private static void UpdateNumberCustomField(
        NumberCustomField numberCustomField,
        UpdateCustomFieldFromProjectTaskCommand command)
    {
        numberCustomField.Update(command.NumberCustomField!.Number.IfNotNull(DecimalNumber.Create));
    }

    private static void UpdateSingleSelectCustomField(
        SingleSelectCustomField singleSelectCustomField,
        UpdateCustomFieldFromProjectTaskCommand command)
    {
        var option = singleSelectCustomField.Setup.Options
            .SingleOrDefault(o => o.Id == command.SingleSelectCustomField!.OptionId);
        singleSelectCustomField.Update(option);
    }

    private static void UpdateTextCustomField(
        TextCustomField textCustomField,
        UpdateCustomFieldFromProjectTaskCommand command)
    {
        textCustomField.Update(command.TextCustomField!.Text.IfNotNull(Text.Create));
    }
}
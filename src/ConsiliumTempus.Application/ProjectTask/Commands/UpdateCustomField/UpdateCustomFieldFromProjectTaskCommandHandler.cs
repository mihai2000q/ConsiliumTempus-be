using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Domain.User.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectTask.Commands.UpdateCustomField;

public sealed class UpdateCustomFieldFromProjectTaskCommandHandler(
    IProjectTaskRepository projectTaskRepository,
    IUserRepository userRepository)
    : IRequestHandler<UpdateCustomFieldFromProjectTaskCommand, ErrorOr<UpdateCustomFieldFromProjectTaskResult>>
{
    private sealed class UserNotFoundException : Exception;

    private sealed class MultiSelectOptionNotFoundException : Exception;

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

        try
        {
            switch (customField)
            {
                case DateCustomField dateCustomField:
                    UpdateDateCustomField(dateCustomField, command);
                    break;

                case DateTimeCustomField dateTimeCustomField:
                    UpdateDateTimeCustomField(dateTimeCustomField, command);
                    break;

                case DurationCustomField durationCustomField:
                    UpdateDurationCustomField(durationCustomField, command);
                    break;

                case MultiSelectCustomField multiSelectCustomField:
                    UpdateMultiSelectCustomField(multiSelectCustomField, command);
                    break;

                case NumberCustomField numberCustomField:
                    UpdateNumberCustomField(numberCustomField, command);
                    break;

                case SingleSelectCustomField singleSelectCustomField:
                    UpdateSingleSelectCustomField(singleSelectCustomField, command);
                    break;

                case PeopleCustomField peopleCustomField:
                    await UpdatePeopleCustomField(peopleCustomField, command, userRepository, cancellationToken);
                    break;

                case TextCustomField textCustomField:
                    UpdateTextCustomField(textCustomField, command);
                    break;

                case TimeCustomField timeCustomField:
                    UpdateTimeCustomField(timeCustomField, command);
                    break;
            }
        }
        catch (UserNotFoundException)
        {
            return Errors.User.NotFound;
        }
        catch (MultiSelectOptionNotFoundException)
        {
            return Errors.MultiSelectOption.NotFound;
        }

        task.RefreshUpdatedDateTime();
        task.Stage.Sprint.Project.RefreshActivity();

        return new UpdateCustomFieldFromProjectTaskResult();
    }

    private static void UpdateDateCustomField(
        DateCustomField dateCustomField,
        UpdateCustomFieldFromProjectTaskCommand command)
    {
        dateCustomField.Update(command.DateCustomField!.Date);
    }

    private static void UpdateDateTimeCustomField(
        DateTimeCustomField dateTimeCustomField,
        UpdateCustomFieldFromProjectTaskCommand command)
    {
        dateTimeCustomField.Update(command.DateTimeCustomField!.DateTime);
    }

    private static void UpdateDurationCustomField(
        DurationCustomField durationCustomField,
        UpdateCustomFieldFromProjectTaskCommand command)
    {
        durationCustomField.Update(command.DurationCustomField!.Duration);
    }

    private static void UpdateMultiSelectCustomField(
        MultiSelectCustomField multiSelectCustomField,
        UpdateCustomFieldFromProjectTaskCommand command)
    {
        var option = multiSelectCustomField.Setup.Options
            .SingleOrDefault(o => o.Id == command.MultiSelectCustomField!.OptionId);
        if (option is null) throw new MultiSelectOptionNotFoundException();

        if (command.MultiSelectCustomField!.Remove)
            multiSelectCustomField.RemoveOption(option);
        else
            multiSelectCustomField.AddOption(option);
    }

    private static void UpdateNumberCustomField(
        NumberCustomField numberCustomField,
        UpdateCustomFieldFromProjectTaskCommand command)
    {
        numberCustomField.Update(command.NumberCustomField!.Number.IfNotNull(DecimalNumber.Create));
    }

    private static async Task UpdatePeopleCustomField(
        PeopleCustomField peopleCustomField,
        UpdateCustomFieldFromProjectTaskCommand command,
        IUserRepository userRepository,
        CancellationToken cancellationToken)
    {
        var person = command.PeopleCustomField!.PersonId is not null
            ? await userRepository.Get(UserId.Create(command.PeopleCustomField!.PersonId.Value), cancellationToken)
            : null;
        if (person is null && command.PeopleCustomField!.PersonId is not null) throw new UserNotFoundException();
        peopleCustomField.Update(person);
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

    private static void UpdateTimeCustomField(
        TimeCustomField timeCustomField,
        UpdateCustomFieldFromProjectTaskCommand command)
    {
        timeCustomField.Update(command.TimeCustomField!.Time);
    }
}
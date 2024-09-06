using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.User;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.CustomFieldSetup.Commands.Update;

public sealed class UpdateCustomFieldSetupCommandHandler(
    ICustomFieldSetupRepository customFieldSetupRepository,
    ICurrentUserProvider currentUserProvider)
    : IRequestHandler<UpdateCustomFieldSetupCommand, ErrorOr<UpdateCustomFieldSetupResult>>
{
    private sealed class SingleSelectOptionNotFoundException : Exception;

    private sealed class MultiSelectOptionNotFoundException : Exception;

    public async Task<ErrorOr<UpdateCustomFieldSetupResult>> Handle(UpdateCustomFieldSetupCommand command,
        CancellationToken cancellationToken)
    {
        var customFieldSetup = await customFieldSetupRepository.GetWithWorkspaceAndProjects(
            CustomFieldSetupId.Create(command.Id),
            cancellationToken);
        if (customFieldSetup is null) return Errors.CustomFieldSetup.NotFound;

        var user = await currentUserProvider.GetCurrentUserAfterPermissionCheck(cancellationToken);

        try
        {
            switch (customFieldSetup)
            {
                case DateCustomFieldSetupAggregate dateSetup:
                    UpdateDateCustomFieldSetup(command, dateSetup, user);
                    break;

                case DateTimeCustomFieldSetupAggregate dateTimeSetup:
                    UpdateDateTimeCustomFieldSetup(command, dateTimeSetup, user);
                    break;

                case DurationCustomFieldSetupAggregate durationSetup:
                    UpdateDurationCustomFieldSetup(command, durationSetup, user);
                    break;

                case MultiSelectCustomFieldSetupAggregate multiSelectSetup:
                    UpdateMultiSelectCustomFieldSetup(command, multiSelectSetup, user);
                    break;

                case NumberCustomFieldSetupAggregate numberSetup:
                    UpdateNumberCustomFieldSetup(command, numberSetup, user);
                    break;

                case PeopleCustomFieldSetupAggregate peopleSetup:
                    UpdatePeopleCustomFieldSetup(command, peopleSetup, user);
                    break;

                case SingleSelectCustomFieldSetupAggregate singleSelectSetup:
                    UpdateSingleSelectCustomFieldSetup(command, singleSelectSetup, user);
                    break;

                case TextCustomFieldSetupAggregate textSetup:
                    UpdateTextCustomFieldSetup(command, textSetup, user);
                    break;

                case TimeCustomFieldSetupAggregate timeSetup:
                    UpdateTimeCustomFieldSetup(command, timeSetup, user);
                    break;
            }
        }
        catch (SingleSelectOptionNotFoundException)
        {
            return Errors.SingleSelectOption.NotFound;
        }
        catch (MultiSelectOptionNotFoundException)
        {
            return Errors.MultiSelectOption.NotFound;
        }

        customFieldSetup.Workspace?.RefreshActivity();
        customFieldSetup.Projects.ForEach(p => p.RefreshActivity());

        return new UpdateCustomFieldSetupResult();
    }

    private static void UpdateDateCustomFieldSetup(
        UpdateCustomFieldSetupCommand command,
        DateCustomFieldSetupAggregate setup,
        UserAggregate user)
    {
        setup.Update(
            command.DateCustomFieldSetup!.DefaultDate,
            Name.Create(command.Name),
            Description.Create(command.Description),
            user);
    }

    private static void UpdateDateTimeCustomFieldSetup(
        UpdateCustomFieldSetupCommand command,
        DateTimeCustomFieldSetupAggregate setup,
        UserAggregate user)
    {
        setup.Update(
            command.DateTimeCustomFieldSetup!.DefaultDateTime,
            Name.Create(command.Name),
            Description.Create(command.Description),
            user);
    }

    private static void UpdateDurationCustomFieldSetup(
        UpdateCustomFieldSetupCommand command,
        DurationCustomFieldSetupAggregate setup,
        UserAggregate user)
    {
        setup.Update(
            command.DurationCustomFieldSetup!.DefaultDuration,
            Name.Create(command.Name),
            Description.Create(command.Description),
            user);
    }

    private static void UpdateMultiSelectCustomFieldSetup(
        UpdateCustomFieldSetupCommand command,
        MultiSelectCustomFieldSetupAggregate setup,
        UserAggregate user)
    {
        setup.Update(
            Name.Create(command.Name),
            Description.Create(command.Description),
            user);

        if (command.MultiSelectCustomFieldSetup!.Operation is null) return;

        UpdateMultiSelectCustomFieldSetupOptions(setup, command.MultiSelectCustomFieldSetup!);
    }

    private static void UpdateNumberCustomFieldSetup(
        UpdateCustomFieldSetupCommand command,
        NumberCustomFieldSetupAggregate setup,
        UserAggregate user)
    {
        setup.Update(
            NumberCustomFieldSettings.Create(
                command.NumberCustomFieldSetup!.Settings.CurrencyCode,
                (short)command.NumberCustomFieldSetup!.Settings.Decimals,
                command.NumberCustomFieldSetup!.Settings.Rounding),
            command.NumberCustomFieldSetup!.DefaultNumber.IfNotNull(DecimalNumber.Create),
            Name.Create(command.Name),
            Description.Create(command.Description),
            user);
    }

    private static void UpdatePeopleCustomFieldSetup(
        UpdateCustomFieldSetupCommand command,
        PeopleCustomFieldSetupAggregate setup,
        UserAggregate user)
    {
        setup.Update(
            Name.Create(command.Name),
            Description.Create(command.Description),
            user);
    }

    private static void UpdateSingleSelectCustomFieldSetup(
        UpdateCustomFieldSetupCommand command,
        SingleSelectCustomFieldSetupAggregate setup,
        UserAggregate user)
    {
        var defaultOption = setup.Options.SingleOrDefault(o =>
            o.Id == command.SingleSelectCustomFieldSetup!.DefaultOptionId);
        if (command.SingleSelectCustomFieldSetup!.DefaultOptionId is not null && defaultOption is null)
            throw new SingleSelectOptionNotFoundException();

        setup.Update(
            defaultOption?.Id,
            Name.Create(command.Name),
            Description.Create(command.Description),
            user);

        if (command.SingleSelectCustomFieldSetup.Operation is null) return;

        UpdateSingleSelectCustomFieldSetupOptions(setup, command.SingleSelectCustomFieldSetup!);
    }

    private static void UpdateTextCustomFieldSetup(
        UpdateCustomFieldSetupCommand command,
        TextCustomFieldSetupAggregate setup,
        UserAggregate user)
    {
        setup.Update(
            command.TextCustomFieldSetup!.DefaultText.IfNotNull(Text.Create),
            Name.Create(command.Name),
            Description.Create(command.Description),
            user);
    }

    private static void UpdateTimeCustomFieldSetup(
        UpdateCustomFieldSetupCommand command,
        TimeCustomFieldSetupAggregate setup,
        UserAggregate user)
    {
        setup.Update(
            command.TimeCustomFieldSetup!.DefaultTime,
            Name.Create(command.Name),
            Description.Create(command.Description),
            user);
    }

    private static void UpdateMultiSelectCustomFieldSetupOptions(
        MultiSelectCustomFieldSetupAggregate setup,
        UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand command)
    {
        switch (command.Operation)
        {
            case UpdateCustomFieldSetupCommand.OptionOperation.Add:
                setup.AddOption(MultiSelectOption.Create(
                    command.NewOption!.Value,
                    command.NewOption!.Color,
                    CustomOrderPosition.Create(setup.Options.Count)));
                break;

            case UpdateCustomFieldSetupCommand.OptionOperation.Update:
                var optionToUpdate = setup.Options
                    .SingleOrDefault(o => o.Id == command.OptionId);
                if (optionToUpdate is null) throw new MultiSelectOptionNotFoundException();
                optionToUpdate.Update(command.NewOption!.Value, command.NewOption!.Color);
                break;

            case UpdateCustomFieldSetupCommand.OptionOperation.Move:
                var optionToMove = setup.Options
                    .SingleOrDefault(o => o.Id == command.OptionId);
                if (optionToMove is null) throw new MultiSelectOptionNotFoundException();

                var overOption = setup.Options
                    .SingleOrDefault(o => o.Id == command.OverOptionId);
                if (overOption is null) throw new MultiSelectOptionNotFoundException();

                setup.MoveOption(optionToMove, overOption);
                break;

            case UpdateCustomFieldSetupCommand.OptionOperation.Remove:
                var optionToRemove = setup.Options
                    .SingleOrDefault(o => o.Id == command.OptionId);
                if (optionToRemove is null) throw new MultiSelectOptionNotFoundException();
                setup.RemoveOption(optionToRemove);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(command));
        }
    }

    private static void UpdateSingleSelectCustomFieldSetupOptions(
        SingleSelectCustomFieldSetupAggregate setup,
        UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand command)
    {
        switch (command.Operation)
        {
            case UpdateCustomFieldSetupCommand.OptionOperation.Add:
                setup.AddOption(SingleSelectOption.Create(
                    command.NewOption!.Value,
                    command.NewOption!.Color,
                    CustomOrderPosition.Create(setup.Options.Count)));
                break;

            case UpdateCustomFieldSetupCommand.OptionOperation.Update:
                var optionToUpdate = setup.Options
                    .SingleOrDefault(o => o.Id == command.OptionId);
                if (optionToUpdate is null) throw new SingleSelectOptionNotFoundException();
                optionToUpdate.Update(command.NewOption!.Value, command.NewOption!.Color);
                break;

            case UpdateCustomFieldSetupCommand.OptionOperation.Move:
                var optionToMove = setup.Options
                    .SingleOrDefault(o => o.Id == command.OptionId);
                if (optionToMove is null) throw new SingleSelectOptionNotFoundException();

                var overOption = setup.Options
                    .SingleOrDefault(o => o.Id == command.OverOptionId);
                if (overOption is null) throw new SingleSelectOptionNotFoundException();

                setup.MoveOption(optionToMove, overOption);
                break;

            case UpdateCustomFieldSetupCommand.OptionOperation.Remove:
                var optionToRemove = setup.Options
                    .SingleOrDefault(o => o.Id == command.OptionId);
                if (optionToRemove is null) throw new SingleSelectOptionNotFoundException();
                setup.RemoveOption(optionToRemove);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(command));
        }
    }
}
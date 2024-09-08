using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;

public sealed class CreateCustomFieldSetupCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IWorkspaceRepository workspaceRepository,
    IProjectRepository projectRepository,
    ICustomFieldSetupRepository customFieldSetupRepository)
    : IRequestHandler<CreateCustomFieldSetupCommand, ErrorOr<CreateCustomFieldSetupResult>>
{
    private WorkspaceAggregate? _workspace;
    private ProjectAggregate? _project;

    public async Task<ErrorOr<CreateCustomFieldSetupResult>> Handle(CreateCustomFieldSetupCommand command,
        CancellationToken cancellationToken)
    {
        if (command.WorkspaceId is not null)
        {
            _workspace = await workspaceRepository.Get(
                WorkspaceId.Create(command.WorkspaceId.Value),
                cancellationToken);
            if (_workspace is null) return Errors.Workspace.NotFound;
        }

        if (command.ProjectId is not null)
        {
            _project = await projectRepository.Get(
                ProjectId.Create(command.ProjectId.Value),
                cancellationToken);
            if (_project is null) return Errors.Project.NotFound;
        }

        var user = await currentUserProvider.GetCurrentUserAfterPermissionCheck(cancellationToken);

        CustomFieldSetupAggregate customFieldSetup = command.Type switch
        {
            CustomFieldType.Date => GetDateCustomFieldSetup(command, user),
            CustomFieldType.DateTime => GetDateTimeCustomFieldSetup(command, user),
            CustomFieldType.Duration => GetDurationCustomFieldSetup(command, user),
            CustomFieldType.MultiSelect => GetMultiSelectCustomFieldSetup(command, user),
            CustomFieldType.Number => GetNumberCustomFieldSetup(command, user),
            CustomFieldType.People => GetPeopleCustomFieldSetup(command, user),
            CustomFieldType.SingleSelect => GetSingleSelectCustomFieldSetup(command, user),
            CustomFieldType.Text => GetTextCustomFieldSetup(command, user),
            CustomFieldType.Time => GetTimeCustomFieldSetup(command, user),
            _ => throw new ArgumentOutOfRangeException(nameof(command))
        };
        await customFieldSetupRepository.Add(customFieldSetup, cancellationToken);
        _workspace?.RefreshActivity();
        _project?.RefreshActivity();

        return new CreateCustomFieldSetupResult();
    }

    private DateCustomFieldSetupAggregate GetDateCustomFieldSetup(
        CreateCustomFieldSetupCommand command,
        UserAggregate user)
    {
        return DateCustomFieldSetupAggregate.Create(
            command.DateCustomFieldSetup!.DefaultDate,
            Name.Create(command.Name),
            Description.Create(command.Description),
            _workspace,
            _project,
            user);
    }

    private DateTimeCustomFieldSetupAggregate GetDateTimeCustomFieldSetup(
        CreateCustomFieldSetupCommand command,
        UserAggregate user)
    {
        return DateTimeCustomFieldSetupAggregate.Create(
            command.DateTimeCustomFieldSetup!.DefaultDateTime,
            Name.Create(command.Name),
            Description.Create(command.Description),
            _workspace,
            _project,
            user);
    }

    private DurationCustomFieldSetupAggregate GetDurationCustomFieldSetup(
        CreateCustomFieldSetupCommand command,
        UserAggregate user)
    {
        return DurationCustomFieldSetupAggregate.Create(
            command.DurationCustomFieldSetup!.DefaultDuration,
            Name.Create(command.Name),
            Description.Create(command.Description),
            _workspace,
            _project,
            user);
    }

    private MultiSelectCustomFieldSetupAggregate GetMultiSelectCustomFieldSetup(
        CreateCustomFieldSetupCommand command,
        UserAggregate user)
    {
        var options = command.MultiSelectCustomFieldSetup!.Options.Select((o, index) =>
                MultiSelectOption.Create(
                    o.Value,
                    o.Color,
                    CustomOrderPosition.Create(index)))
            .ToList();

        return MultiSelectCustomFieldSetupAggregate.Create(
            options,
            Name.Create(command.Name),
            Description.Create(command.Description),
            _workspace,
            _project,
            user);
    }

    private NumberCustomFieldSetupAggregate GetNumberCustomFieldSetup(
        CreateCustomFieldSetupCommand command,
        UserAggregate user)
    {
        return NumberCustomFieldSetupAggregate.Create(
            NumberCustomFieldSettings.Create(
                command.NumberCustomFieldSetup!.Settings.CurrencyCode,
                (short)command.NumberCustomFieldSetup!.Settings.Decimals,
                command.NumberCustomFieldSetup!.Settings.Rounding),
            command.NumberCustomFieldSetup!.DefaultNumber.IfNotNull(DecimalNumber.Create),
            Name.Create(command.Name),
            Description.Create(command.Description),
            _workspace,
            _project,
            user);
    }

    private PeopleCustomFieldSetupAggregate GetPeopleCustomFieldSetup(
        CreateCustomFieldSetupCommand command,
        UserAggregate user)
    {
        return PeopleCustomFieldSetupAggregate.Create(
            Name.Create(command.Name),
            Description.Create(command.Description),
            _workspace,
            _project,
            user);
    }

    private SingleSelectCustomFieldSetupAggregate GetSingleSelectCustomFieldSetup(
        CreateCustomFieldSetupCommand command,
        UserAggregate user)
    {
        var options = command.SingleSelectCustomFieldSetup!.Options.Select((o, index) =>
                SingleSelectOption.Create(
                    o.Value,
                    o.Color,
                    CustomOrderPosition.Create(index)))
            .ToList();

        var defaultOption = command.SingleSelectCustomFieldSetup!.DefaultOptionId.IfNotNull(optionId =>
        {
            var optionIndex = command.SingleSelectCustomFieldSetup.Options
                .FindIndex(x => x.Id == optionId);
            return options[optionIndex];
        });

        return SingleSelectCustomFieldSetupAggregate.Create(
            options,
            defaultOption?.Id,
            Name.Create(command.Name),
            Description.Create(command.Description),
            _workspace,
            _project,
            user);
    }

    private TextCustomFieldSetupAggregate GetTextCustomFieldSetup(
        CreateCustomFieldSetupCommand command,
        UserAggregate user)
    {
        return TextCustomFieldSetupAggregate.Create(
            command.TextCustomFieldSetup!.DefaultText.IfNotNull(Text.Create),
            Name.Create(command.Name),
            Description.Create(command.Description),
            _workspace,
            _project,
            user);
    }

    private TimeCustomFieldSetupAggregate GetTimeCustomFieldSetup(
        CreateCustomFieldSetupCommand command,
        UserAggregate user)
    {
        return TimeCustomFieldSetupAggregate.Create(
            command.TimeCustomFieldSetup!.DefaultTime,
            Name.Create(command.Name),
            Description.Create(command.Description),
            _workspace,
            _project,
            user);
    }
}
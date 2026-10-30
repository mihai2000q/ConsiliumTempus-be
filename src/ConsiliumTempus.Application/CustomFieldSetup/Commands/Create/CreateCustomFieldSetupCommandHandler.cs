using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;

internal sealed class CreateCustomFieldSetupCommandHandler(
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

        CustomFieldSetupAggregate customFieldSetup = command.Type switch
        {
            CustomFieldType.Date => GetDateCustomFieldSetup(command),
            CustomFieldType.DateTime => GetDateTimeCustomFieldSetup(command),
            CustomFieldType.Duration => GetDurationCustomFieldSetup(command),
            CustomFieldType.MultiSelect => GetMultiSelectCustomFieldSetup(command),
            CustomFieldType.Number => GetNumberCustomFieldSetup(command),
            CustomFieldType.People => GetPeopleCustomFieldSetup(command),
            CustomFieldType.SingleSelect => GetSingleSelectCustomFieldSetup(command),
            CustomFieldType.Text => GetTextCustomFieldSetup(command),
            CustomFieldType.Time => GetTimeCustomFieldSetup(command),
            _ => throw new ArgumentOutOfRangeException(nameof(command))
        };
        await customFieldSetupRepository.Add(customFieldSetup, cancellationToken);
        _workspace?.RefreshActivity();
        _project?.RefreshActivity();

        return new CreateCustomFieldSetupResult();
    }

    private DateCustomFieldSetupAggregate GetDateCustomFieldSetup(
        CreateCustomFieldSetupCommand command)
    {
        return DateCustomFieldSetupAggregate.Create(
            command.DateCustomFieldSetup!.DefaultDate,
            Name.Create(command.Name),
            Description.Create(command.Description),
            _workspace,
            _project);
    }

    private DateTimeCustomFieldSetupAggregate GetDateTimeCustomFieldSetup(
        CreateCustomFieldSetupCommand command)
    {
        return DateTimeCustomFieldSetupAggregate.Create(
            command.DateTimeCustomFieldSetup!.DefaultDateTime,
            Name.Create(command.Name),
            Description.Create(command.Description),
            _workspace,
            _project);
    }

    private DurationCustomFieldSetupAggregate GetDurationCustomFieldSetup(
        CreateCustomFieldSetupCommand command)
    {
        return DurationCustomFieldSetupAggregate.Create(
            command.DurationCustomFieldSetup!.DefaultDuration,
            Name.Create(command.Name),
            Description.Create(command.Description),
            _workspace,
            _project);
    }

    private MultiSelectCustomFieldSetupAggregate GetMultiSelectCustomFieldSetup(
        CreateCustomFieldSetupCommand command)
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
            _project);
    }

    private NumberCustomFieldSetupAggregate GetNumberCustomFieldSetup(
        CreateCustomFieldSetupCommand command)
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
            _project);
    }

    private PeopleCustomFieldSetupAggregate GetPeopleCustomFieldSetup(
        CreateCustomFieldSetupCommand command)
    {
        return PeopleCustomFieldSetupAggregate.Create(
            Name.Create(command.Name),
            Description.Create(command.Description),
            _workspace,
            _project);
    }

    private SingleSelectCustomFieldSetupAggregate GetSingleSelectCustomFieldSetup(
        CreateCustomFieldSetupCommand command)
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
            _project);
    }

    private TextCustomFieldSetupAggregate GetTextCustomFieldSetup(
        CreateCustomFieldSetupCommand command)
    {
        return TextCustomFieldSetupAggregate.Create(
            command.TextCustomFieldSetup!.DefaultText.IfNotNull(Text.Create),
            Name.Create(command.Name),
            Description.Create(command.Description),
            _workspace,
            _project);
    }

    private TimeCustomFieldSetupAggregate GetTimeCustomFieldSetup(
        CreateCustomFieldSetupCommand command)
    {
        return TimeCustomFieldSetupAggregate.Create(
            command.TimeCustomFieldSetup!.DefaultTime,
            Name.Create(command.Name),
            Description.Create(command.Description),
            _workspace,
            _project);
    }
}
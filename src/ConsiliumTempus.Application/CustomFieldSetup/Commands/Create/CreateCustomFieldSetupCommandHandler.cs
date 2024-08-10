using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.Entities;
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

        var customFieldType = Enum.Parse<CustomFieldType>(command.Type, true);
        CustomFieldSetupAggregate customFieldSetup = customFieldType switch
        {
            CustomFieldType.Number => GetNumberCustomFieldSetup(command, user),
            CustomFieldType.SingleSelect => GetSingleSelectCustomFieldSetup(command, user),
            CustomFieldType.Text => GetTextCustomFieldSetup(command, user),
            _ => throw new ArgumentOutOfRangeException(nameof(command))
        };
        await customFieldSetupRepository.Add(customFieldSetup, cancellationToken);

        return new CreateCustomFieldSetupResult();
    }

    private NumberCustomFieldSetupAggregate GetNumberCustomFieldSetup(
        CreateCustomFieldSetupCommand command,
        UserAggregate user)
    {
        return NumberCustomFieldSetupAggregate.Create(
            NumberCustomFieldSettings.Create(
                command.NumberSettings!.CurrencyCode,
                (short)command.NumberSettings!.Decimals,
                command.NumberSettings!.Rounding),
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
        return SingleSelectCustomFieldSetupAggregate.Create(
            command.SingleSelectOptions!.Select((o, index) =>
                    SingleSelectOption.Create(
                        o.Value,
                        o.Color,
                        index))
                .ToList(),
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
            Name.Create(command.Name),
            Description.Create(command.Description),
            _workspace,
            _project,
            user);
    }
}
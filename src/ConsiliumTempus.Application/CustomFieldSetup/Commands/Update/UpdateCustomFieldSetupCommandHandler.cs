using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
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
    public async Task<ErrorOr<UpdateCustomFieldSetupResult>> Handle(UpdateCustomFieldSetupCommand command,
        CancellationToken cancellationToken)
    {
        var customFieldSetup = await customFieldSetupRepository.GetWithWorkspaceAndProjects(
            CustomFieldSetupId.Create(command.Id),
            cancellationToken);
        if (customFieldSetup is null) return Errors.CustomFieldSetup.NotFound;

        var user = await currentUserProvider.GetCurrentUserAfterPermissionCheck(cancellationToken);

        switch (customFieldSetup)
        {
            case NumberCustomFieldSetupAggregate numberSetup:
                UpdateNumberCustomFieldSetup(command, numberSetup, user);
                break;
            case SingleSelectCustomFieldSetupAggregate singleSelectSetup:
                UpdateSingleSelectCustomFieldSetup(command, singleSelectSetup, user);
                break;
            case TextCustomFieldSetupAggregate textSetup:
                UpdateTextCustomFieldSetup(command, textSetup, user);
                break;
        }
        customFieldSetup.Workspace?.RefreshActivity();
        customFieldSetup.Projects.ForEach(p => p.RefreshActivity());

        return new UpdateCustomFieldSetupResult();
    }

    private static void UpdateNumberCustomFieldSetup(
        UpdateCustomFieldSetupCommand command,
        NumberCustomFieldSetupAggregate numberCustomField,
        UserAggregate user)
    {
        numberCustomField.Update(
            NumberCustomFieldSettings.Create(
                command.NumberCustomFieldSetup!.Settings.CurrencyCode,
                (short)command.NumberCustomFieldSetup!.Settings.Decimals,
                command.NumberCustomFieldSetup!.Settings.Rounding),
            command.NumberCustomFieldSetup!.DefaultNumber.IfNotNull(DecimalNumber.Create),
            Name.Create(command.Name),
            Description.Create(command.Description),
            user);
    }

    private static void UpdateSingleSelectCustomFieldSetup(
        UpdateCustomFieldSetupCommand command,
        SingleSelectCustomFieldSetupAggregate singleSelectCustomFieldSetup,
        UserAggregate user)
    {
        var defaultOption = singleSelectCustomFieldSetup.Options.SingleOrDefault(o =>
            o.Id == command.SingleSelectCustomFieldSetup!.DefaultOptionId);

        singleSelectCustomFieldSetup.Update(
            defaultOption,
            Name.Create(command.Name),
            Description.Create(command.Description),
            user);
    }

    private static void UpdateTextCustomFieldSetup(
        UpdateCustomFieldSetupCommand command,
        TextCustomFieldSetupAggregate textCustomFieldSetup,
        UserAggregate user)
    {
        textCustomFieldSetup.Update(
            command.TextCustomFieldSetup!.DefaultText.IfNotNull(Text.Create),
            Name.Create(command.Name),
            Description.Create(command.Description),
            user);
    }
}
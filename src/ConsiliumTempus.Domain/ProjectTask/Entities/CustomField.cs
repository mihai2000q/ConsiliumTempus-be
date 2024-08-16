using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;

namespace ConsiliumTempus.Domain.ProjectTask.Entities;

public abstract class CustomField : Entity<CustomFieldId>
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    protected CustomField()
    {
    }

    protected CustomField(CustomFieldId id) : base(id)
    {
    }

    public static CustomField Create(CustomFieldSetupAggregate setup)
    {
        return setup switch
        {
            NumberCustomFieldSetupAggregate numberSetup =>
                NumberCustomField.Create(
                    null,
                    numberSetup),
            SingleSelectCustomFieldSetupAggregate singleSelectSetup =>
                SingleSelectCustomField.Create(
                    null,
                    singleSelectSetup),
            TextCustomFieldSetupAggregate textSetup =>
                TextCustomField.Create(
                    null,
                    textSetup),
            _ => throw new ArgumentOutOfRangeException(nameof(setup), setup, "Type Not Supported")
        };
    }
}
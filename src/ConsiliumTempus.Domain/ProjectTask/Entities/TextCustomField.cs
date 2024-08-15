using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;

namespace ConsiliumTempus.Domain.ProjectTask.Entities;

public sealed class TextCustomField : CustomField
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private TextCustomField()
    {
    }

    private TextCustomField(
        Text? text,
        TextCustomFieldSetupAggregate setup,
        CustomFieldId id,
        ProjectTaskAggregate task) : base(id, task)
    {
        Text = text;
        Setup = setup;
    }

    public Text? Text { get; init; }
    public TextCustomFieldSetupAggregate Setup { get; init; } = null!;

    public static TextCustomField Create(
        Text text,
        TextCustomFieldSetupAggregate setup,
        ProjectTaskAggregate task)
    {
        return new TextCustomField(
            text,
            setup,
            CustomFieldId.CreateUnique(),
            task);
    }
}
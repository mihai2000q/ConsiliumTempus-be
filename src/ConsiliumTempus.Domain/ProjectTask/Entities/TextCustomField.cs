using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.ValueObjects;
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
        ProjectTaskAggregate projectTask) : base(id, projectTask)
    {
        Text = text;
        Setup = setup;
    }

    public Text? Text { get; private set; }
    public override TextCustomFieldSetupAggregate Setup { get; } = null!;

    public static TextCustomField Create(
        Text? text,
        TextCustomFieldSetupAggregate setup,
        ProjectTaskAggregate projectTask)
    {
        return new TextCustomField(
            text,
            setup,
            CustomFieldId.CreateUnique(),
            projectTask);
    }
    
    public void Update(Text? text)
    {
        Text = text;
    }
}
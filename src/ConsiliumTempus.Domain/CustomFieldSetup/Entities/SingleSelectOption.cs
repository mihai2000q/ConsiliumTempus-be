using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.CustomFieldSetup.Entities;

public sealed class SingleSelectOption : Entity<Guid>
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private SingleSelectOption()
    {
    }

    private SingleSelectOption(
        Guid id,
        string value,
        string color,
        int customOrderPosition) : base(id)
    {
        Value = value;
        Color = color;
        CustomOrderPosition = customOrderPosition;
    }

    public string Value { get; } = string.Empty;
    public string Color { get; } = string.Empty;
    public int CustomOrderPosition { get; }

    public static SingleSelectOption Create(
        string value,
        string color,
        int customOrderPosition)
    {
        return new SingleSelectOption(
            Guid.NewGuid(),
            value,
            color,
            customOrderPosition);
    }
}
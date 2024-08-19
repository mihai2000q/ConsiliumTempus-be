using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.ValueObjects;

namespace ConsiliumTempus.Domain.Common.Entities;

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
        CustomOrderPosition customOrderPosition) : base(id)
    {
        Value = value;
        Color = color;
        CustomOrderPosition = customOrderPosition;
    }

    public string Value { get; private set; } = string.Empty;
    public string Color { get; private set; } = string.Empty;
    public CustomOrderPosition CustomOrderPosition { get; private set; } = null!;

    public static SingleSelectOption Create(
        string value,
        string color,
        CustomOrderPosition customOrderPosition)
    {
        return new SingleSelectOption(
            Guid.NewGuid(),
            value,
            color,
            customOrderPosition);
    }
    }
}
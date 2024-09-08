using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.ValueObjects;

namespace ConsiliumTempus.Domain.Common.Entities;

public sealed class MultiSelectOption : Entity<Guid>
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private MultiSelectOption()
    {
    }

    private MultiSelectOption(
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

    public static MultiSelectOption Create(
        string value,
        string color,
        CustomOrderPosition customOrderPosition)
    {
        return new MultiSelectOption(
            Guid.NewGuid(),
            value,
            color,
            customOrderPosition);
    }

    public void Update(string value, string color)
    {
        Value = value;
        Color = color;
    }

    public void UpdateCustomOrderPosition(CustomOrderPosition customOrderPosition)
    {
        CustomOrderPosition = customOrderPosition;
    }
}
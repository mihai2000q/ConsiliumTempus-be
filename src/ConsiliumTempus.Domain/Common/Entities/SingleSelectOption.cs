using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

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
        int orderPosition) : base(id)
    {
        Value = value;
        Color = color;
        OrderPosition = orderPosition;
    }

    public string Value { get; } = string.Empty;
    public string Color { get; } = string.Empty;
    public int OrderPosition { get; }

    public static SingleSelectOption Create(
        string value,
        string color,
        int orderPosition)
    {
        return new SingleSelectOption(
            Guid.NewGuid(),
            value,
            color,
            orderPosition);
    }
}
using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.ProjectTask.ValueObjects;

public sealed class SelectedOption : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private SelectedOption()
    {
    }

    private SelectedOption(string value, string color)
    {
        Value = value;
        Color = color;
    }

    public string Value { get; } = string.Empty;
    public string Color { get; } = string.Empty;

    public static SelectedOption Create(string value, string color)
    {
        return new SelectedOption(value, color);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return Color;
    }
}
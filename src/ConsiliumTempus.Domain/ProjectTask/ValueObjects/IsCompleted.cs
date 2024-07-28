using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.ProjectTask.ValueObjects;

public sealed class IsCompleted : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private IsCompleted()
    {
    }

    private IsCompleted(bool value, DateTime? completedOn)
    {
        Value = value;
        CompletedOn = completedOn;
    }

    public bool Value { get; }
    public DateTime? CompletedOn { get; }

    public static IsCompleted Create(bool value, DateTime? completedOn = null)
    {
        return new IsCompleted(
            value, 
            completedOn);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
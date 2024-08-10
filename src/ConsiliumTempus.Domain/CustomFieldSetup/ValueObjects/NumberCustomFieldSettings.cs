using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;

public sealed class NumberCustomFieldSettings : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private NumberCustomFieldSettings()
    {
    }

    private NumberCustomFieldSettings(
        string currencyCode, 
        short decimals,
        bool rounding)
    {
        CurrencyCode = currencyCode;
        Decimals = decimals;
        Rounding = rounding;
    }

    public string CurrencyCode { get; init; } = string.Empty;
    public short Decimals { get; init; }
    public bool Rounding { get; init; }

    public static NumberCustomFieldSettings Create(
        string currencyCode, 
        short decimals,
        bool rounding)
    {
        return new NumberCustomFieldSettings(
            currencyCode,
            decimals, 
            rounding);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CurrencyCode;
        yield return Decimals;
        yield return Rounding;
    }
}
using FluentAssertions.Collections;

namespace ConsiliumTempus.Application.UnitTests.TestUtils;

internal static partial class Utils
{
    // FluentAssertions 7.x AllSatisfy fails on empty collections, while generated test data may legitimately be empty
    internal static void AllSatisfyOrEmpty<T>(this GenericCollectionAssertions<T> assertions, Action<T> inspector)
    {
        if (assertions.Subject.Any())
        {
            assertions.AllSatisfy(inspector);
        }
    }

    internal static void AssertEach<TActual, TExpected>(
        this IEnumerable<TActual> actual,
        IEnumerable<TExpected> expected,
        Action<TActual, TExpected> inspector)
    {
        var actualList = actual.ToList();
        var expectedList = expected.ToList();

        actualList.Should().HaveSameCount(expectedList);
        foreach (var (actualItem, expectedItem) in actualList.Zip(expectedList))
        {
            inspector(actualItem, expectedItem);
        }
    }
}

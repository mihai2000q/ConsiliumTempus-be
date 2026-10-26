namespace ConsiliumTempus.Api.UnitTests.TestUtils;

internal static partial class Utils
{
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

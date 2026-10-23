using FluentAssertions.Collections;

namespace ConsiliumTempus.Api.IntegrationTests.TestUtils;

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
}

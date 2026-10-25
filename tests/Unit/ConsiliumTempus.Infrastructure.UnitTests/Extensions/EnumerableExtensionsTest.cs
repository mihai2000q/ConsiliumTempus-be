using ConsiliumTempus.Infrastructure.Extensions;

namespace ConsiliumTempus.Infrastructure.UnitTests.Extensions;

public class EnumerableExtensionsTest
{
    [Fact]
    public void ForEach_ShouldInvokeTheActionForEachItemInOrder()
    {
        // Arrange
        var items = new[] { 3, 1, 2 };
        var visited = new List<int>();

        // Act
        items.ForEach(visited.Add);

        // Assert
        visited.Should().Equal(3, 1, 2);
    }

    [Fact]
    public void ForEach_WhenThereAreNoItems_ShouldNotInvokeTheAction()
    {
        // Arrange
        var action = Substitute.For<Action<int>>();

        // Act
        Array.Empty<int>().ForEach(action);

        // Assert
        action.DidNotReceive().Invoke(Arg.Any<int>());
    }
}

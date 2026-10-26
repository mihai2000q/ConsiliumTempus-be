using ConsiliumTempus.Application.Common.Extensions;

namespace ConsiliumTempus.Application.UnitTests.Common.Extensions;

public class TaskExtensionsTest
{
    #region Setup

    private static async Task<(T1, T2)> Await<T1, T2>(Task<T1> first, Task<T2> second)
    {
        return await (first, second);
    }

    #endregion

    [Fact]
    public async Task GetAwaiter_WhenTasksAreCompleted_ShouldReturnTheirResults()
    {
        // Arrange
        var first = Task.FromResult(1);
        var second = Task.FromResult("two");

        // Act
        var (firstResult, secondResult) = await (first, second);

        // Assert
        firstResult.Should().Be(1);
        secondResult.Should().Be("two");
    }

    [Fact]
    public async Task GetAwaiter_WhenOneTaskIsPending_ShouldWaitForBoth()
    {
        // Arrange
        var first = Task.FromResult(1);
        var pending = new TaskCompletionSource<string>();

        // Act
        var outcome = Await(first, pending.Task);

        // Assert
        outcome.IsCompleted.Should().BeFalse();

        pending.SetResult("two");
        (await outcome).Should().Be((1, "two"));
    }

    [Fact]
    public async Task GetAwaiter_WhenOneTaskFails_ShouldThrowItsException()
    {
        // Arrange
        var first = Task.FromResult(1);
        var failed = Task.FromException<string>(new InvalidOperationException("It failed"));

        // Act
        var outcome = async () => await Await(first, failed);

        // Assert
        await outcome.Should().ThrowAsync<InvalidOperationException>().WithMessage("It failed");
    }
}

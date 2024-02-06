namespace ConsiliumTempus.Application.UnitTests.TestUtils;

public static partial class Utils
{
    public static void ValidateError<T>(this ErrorOr<T> error, Error expectedError)
    {
        error.IsError.Should().BeTrue();
        error.Errors.Should().HaveCount(1);
        error.FirstError.Should().Be(expectedError);
    }
}
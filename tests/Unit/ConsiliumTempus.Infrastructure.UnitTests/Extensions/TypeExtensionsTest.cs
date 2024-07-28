using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Infrastructure.Extensions;

namespace ConsiliumTempus.Infrastructure.UnitTests.Extensions;

public class TypeExtensionsTest
{
    [Theory]
    [InlineData(typeof(ProjectStage), "projectStageId")]
    [InlineData(typeof(ProjectAggregate), "projectId")]
    [InlineData(typeof(ProjectSprintAggregate), "projectSprintId")]
    public void ToCamelId_ShouldReturnStringWithCamelCaseNotationAndSuffixId(Type type, string expected)
    {
        // Arrange - parameterized

        // Act
        var outcome = type.ToCamelId();

        // Assert
        outcome.Should().Be(expected);
    }
}
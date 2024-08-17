using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.CustomFieldSetup.Queries.Get;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.CustomFieldSetup.Queries.Get;

public class GetCustomFieldSetupQueryHandlerTest
{
    #region Setup

    private readonly ICustomFieldSetupRepository _customFieldSetupRepository;
    private readonly GetCustomFieldSetupQueryHandler _uut;

    public GetCustomFieldSetupQueryHandlerTest()
    {
        _customFieldSetupRepository = Substitute.For<ICustomFieldSetupRepository>();
        _uut = new GetCustomFieldSetupQueryHandler(_customFieldSetupRepository);
    }

    #endregion

    [Fact]
    public async Task HandleGetCustomFieldSetupQuery_WhenIsSuccessful_ShouldReturnCustomFieldSetup()
    {
        // Arrange
        var query = CustomFieldSetupQueryFactory.CreateGetCustomFieldSetupQuery();

        var customFieldSetup = CustomFieldSetupFactory.CreateNumber();
        _customFieldSetupRepository
            .Get(Arg.Any<CustomFieldSetupId>())
            .Returns(customFieldSetup);

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _customFieldSetupRepository
            .Received(1)
            .Get(Arg.Is<CustomFieldSetupId>(cId => cId.Value == query.Id));

        outcome.IsError.Should().BeFalse();
        outcome.Value.CustomFieldSetup.Should().Be(customFieldSetup);
    }

    [Fact]
    public async Task HandleGetCustomFieldSetupQuery_WhenIsNull_ShouldReturnNotFoundError()
    {
        // Arrange
        var query = CustomFieldSetupQueryFactory.CreateGetCustomFieldSetupQuery();

        _customFieldSetupRepository
            .Get(Arg.Any<CustomFieldSetupId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _customFieldSetupRepository
            .Received(1)
            .Get(Arg.Is<CustomFieldSetupId>(cId => cId.Value == query.Id));

        outcome.ValidateError(Errors.CustomFieldSetup.NotFound);
    }
}
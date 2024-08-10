using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.CustomFieldSetup.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.CustomFieldSetup.Queries.GetCollection;

public class GetCollectionCustomFieldSetupQueryHandlerTest
{
    #region Setup

    private readonly ICustomFieldSetupRepository _customFieldSetupRepository;
    private readonly GetCollectionCustomFieldSetupQueryHandler _uut;

    public GetCollectionCustomFieldSetupQueryHandlerTest()
    {
        _customFieldSetupRepository = Substitute.For<ICustomFieldSetupRepository>();
        _uut = new GetCollectionCustomFieldSetupQueryHandler(_customFieldSetupRepository);
    }

    #endregion

    [Fact]
    public async Task HandleGetCollectionCustomFieldSetupQuery_WhenIsSuccessful_ShouldReturnCustomFieldSetup()
    {
        // Arrange
        var query = CustomFieldSetupQueryFactory.CreateGetCollectionCustomFieldSetupQuery();

        var customFieldSetups = CustomFieldSetupFactory.CreateList();
        _customFieldSetupRepository
            .GetList(Arg.Any<WorkspaceId?>(), Arg.Any<ProjectId?>())
            .Returns(customFieldSetups);

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _customFieldSetupRepository
            .Received(1)
            .GetList(Arg.Is<WorkspaceId?>(wId => query.WorkspaceId != null ? wId!.Value == query.WorkspaceId : wId == null), 
                Arg.Is<ProjectId?>(pId => query.ProjectId != null ? pId!.Value == query.ProjectId : pId == null));

        outcome.IsError.Should().BeFalse();
        outcome.Value.CustomFieldSetups.Should().BeEquivalentTo(customFieldSetups);
    }
}
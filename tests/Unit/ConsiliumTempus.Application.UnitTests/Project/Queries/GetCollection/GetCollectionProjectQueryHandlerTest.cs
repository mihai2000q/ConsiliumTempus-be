using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.Project.Queries.GetCollection;
using ConsiliumTempus.Application.UnitTests.TestData.Project.Queries.GetCollection;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.Project.Queries.GetCollection;

public class GetCollectionProjectQueryHandlerTest
{
    #region Setup

    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IProjectRepository _projectRepository;
    private readonly GetCollectionProjectQueryHandler _uut;

    public GetCollectionProjectQueryHandlerTest()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _projectRepository = Substitute.For<IProjectRepository>();
        _uut = new GetCollectionProjectQueryHandler(_currentUserProvider, _projectRepository);
    }

    #endregion

    [Theory]
    [ClassData(typeof(GetCollectionProjectQueryHandlerData.GetQueries))]
    public async Task GetCollectionProject_WhenSucceeds_ShouldReturnProjects(GetCollectionProjectQuery query)
    {
        // Arrange
        var user = UserFactory.Create();
        _currentUserProvider
            .GetCurrentUserAfterPermissionCheck()
            .Returns(user);

        var projects = ProjectFactory.CreateList();
        _projectRepository
            .GetListByUser(
                Arg.Any<UserId>(),
                Arg.Any<PaginationInfo>(),
                Arg.Any<IOrder<ProjectAggregate>>(),
                Arg.Any<IReadOnlyList<IFilter<ProjectAggregate>>>())
            .Returns(projects);

        const int projectsCount = 25;
        _projectRepository
            .GetListByUserCount(
                Arg.Any<UserId>(),
                Arg.Any<IEnumerable<IFilter<ProjectAggregate>>>())
            .Returns(projectsCount);

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _currentUserProvider
            .Received(1)
            .GetCurrentUserAfterPermissionCheck();
        await _projectRepository
            .Received(1)
            .GetListByUser(
                Arg.Is<UserId>(uId => uId == user.Id),
                Arg.Is<PaginationInfo>(pi => pi.Assert(query.PageSize, query.CurrentPage)),
                Arg.Is<IOrder<ProjectAggregate>?>(o => Utils.Project.AssertGetCollectionProjectOrder(o, query)),
                Arg.Is<IReadOnlyList<IFilter<ProjectAggregate>>>(filters =>
                    Utils.Project.AssertGetCollectionProjectFilters(filters, query)));

        await _projectRepository
            .GetListByUserCount(
                Arg.Is<UserId>(uId => uId == user.Id),
                Arg.Is<IReadOnlyList<IFilter<ProjectAggregate>>>(filters =>
                    Utils.Project.AssertGetCollectionProjectFilters(filters, query)));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Projects.Should().BeEquivalentTo(projects);
        outcome.Value.TotalCount.Should().Be(projectsCount);
        if (query.PageSize is null || query.CurrentPage is null)
            outcome.Value.TotalPages.Should().BeNull();
        else
            outcome.Value.TotalPages.Should().Be(projectsCount / query.PageSize);
    }
}
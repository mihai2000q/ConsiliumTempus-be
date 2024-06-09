using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.Project.Queries.GetCollection;
using ConsiliumTempus.Application.UnitTests.TestData.Project.Queries.GetCollection;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Filters;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.Orders;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

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
                Arg.Any<WorkspaceId?>(),
                Arg.Any<PaginationInfo>(),
                Arg.Any<IReadOnlyList<IOrder<ProjectAggregate>>>(),
                Arg.Any<IReadOnlyList<IFilter<ProjectAggregate>>>())
            .Returns(projects);

        const int projectsCount = 25;
        _projectRepository
            .GetListByUserCount(
                Arg.Any<UserId>(),
                Arg.Any<WorkspaceId?>(),
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
                Arg.Is<WorkspaceId?>(wId => wId == null ? query.WorkspaceId == null : wId.Value == query.WorkspaceId),
                Arg.Is<PaginationInfo>(pi => pi.AssertPagination(query.PageSize, query.CurrentPage)),
                Arg.Is<IReadOnlyList<IOrder<ProjectAggregate>>>(o =>
                    o.AssertOrders(query.OrderBy, ProjectOrder.OrderProperties)),
                Arg.Is<IReadOnlyList<IFilter<ProjectAggregate>>>(filters =>
                    filters.AssertFilters(query.Search, ProjectFilter.FilterProperties)));
        await _projectRepository
            .Received(1)
            .GetListByUserCount(
                Arg.Is<UserId>(uId => uId == user.Id),
                Arg.Is<WorkspaceId?>(wId => wId == null ? query.WorkspaceId == null : wId.Value == query.WorkspaceId),
                Arg.Is<IReadOnlyList<IFilter<ProjectAggregate>>>(filters =>
                    filters.AssertFilters(query.Search, ProjectFilter.FilterProperties)));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Projects.Should().BeEquivalentTo(projects);
        outcome.Value.TotalCount.Should().Be(projectsCount);
        outcome.Value.CurrentUser.Should().Be(user);
    }
}
using System.Linq.Expressions;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.UnitTests.TestData.Workspace.Queries.GetCollection;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.Filters;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.Orders;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Queries.GetCollection;

public class GetCollectionWorkspaceQueryHandlerTest
{
    #region Setup

    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly GetCollectionWorkspaceQueryHandler _uut;

    public GetCollectionWorkspaceQueryHandlerTest()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _uut = new GetCollectionWorkspaceQueryHandler(_currentUserProvider, _workspaceRepository);
    }

    #endregion

    [Theory]
    [ClassData(typeof(GetCollectionWorkspaceQueryHandlerData.GetQueries))]
    public async Task HandleGetCollectionWorkspaceQuery_WhenIsSuccessful_ShouldReturnCollectionOfWorkspaces(
        GetCollectionWorkspaceQuery query)
    {
        // Arrange
        var user = UserFactory.Create();
        _currentUserProvider
            .GetCurrentUser()
            .Returns(user);

        var workspaces = WorkspaceFactory.CreateList();
        _workspaceRepository
            .GetListByUser(
                Arg.Any<UserAggregate>(),
                Arg.Any<PaginationInfo?>(),
                Arg.Any<IReadOnlyList<IOrder<WorkspaceAggregate>>>(),
                Arg.Any<IEnumerable<IFilter<WorkspaceAggregate>>>())
            .Returns(workspaces);

        const int workspacesCount = 25;
        _workspaceRepository
            .GetListByUserCount(
                Arg.Any<UserAggregate>(),
                Arg.Any<IEnumerable<IFilter<WorkspaceAggregate>>>())
            .Returns(workspacesCount);

        // Act
        var outcome = await _uut.Handle(query, CancellationToken.None);

        // Assert
        await _currentUserProvider
            .Received(1)
            .GetCurrentUser();
        await _workspaceRepository
            .Received(1)
            .GetListByUser(
                Arg.Is<UserAggregate>(u => u == user),
                Arg.Is<PaginationInfo?>(p => p.AssertPagination(query.PageSize, query.CurrentPage)),
                Arg.Is<IReadOnlyList<IOrder<WorkspaceAggregate>>>(o =>
                    o.AssertOrders(query.OrderBy, WorkspaceOrder.OrderProperties)),
                Arg.Is<IReadOnlyList<IFilter<WorkspaceAggregate>>>(filters =>
                    filters.AssertFilters(query.Search, WorkspaceFilter.FilterProperties)));

        await _workspaceRepository
            .Received(1)
            .GetListByUserCount(
                Arg.Is<UserAggregate>(u => u == user),
                Arg.Is<IReadOnlyList<IFilter<WorkspaceAggregate>>>(filters =>
                    filters.AssertFilters(query.Search, WorkspaceFilter.FilterProperties)));

        outcome.IsError.Should().BeFalse();
        Utils.Workspace.AssertGetCollectionResult(
            outcome.Value,
            query,
            workspaces,
            workspacesCount,
            user);
    }

    [Fact]
    public async Task HandleGetCollectionWorkspaceQuery_WhenIsPersonalFirst_ShouldOrderFirstByThePersonalWorkspaceOwnedByTheUser()
    {
        // Arrange
        var user = UserFactory.Create();
        _currentUserProvider
            .GetCurrentUser()
            .Returns(user);

        IReadOnlyList<IOrder<WorkspaceAggregate>>? orders = null;
        _workspaceRepository
            .GetListByUser(
                Arg.Any<UserAggregate>(),
                Arg.Any<PaginationInfo?>(),
                Arg.Do<IReadOnlyList<IOrder<WorkspaceAggregate>>>(o => orders = o),
                Arg.Any<IEnumerable<IFilter<WorkspaceAggregate>>>())
            .Returns(WorkspaceFactory.CreateList());

        var query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
            isPersonalFirst: true,
            orderBy: ["name.asc", "last_activity.desc"]);

        // Act
        var outcome = await _uut.Handle(query, CancellationToken.None);

        // Assert
        outcome.IsError.Should().BeFalse();
        orders.Should().NotBeNull();
        orders.Select(o => o.Type).Should().Equal(OrderType.Descending, OrderType.Ascending, OrderType.Descending);
        orders[1].PropertySelector.Should().BeSameAs(OrderSelector("name"));
        orders[2].PropertySelector.Should().BeSameAs(OrderSelector("last_activity"));

        var isPersonalSelector = orders[0].PropertySelector.Compile();
        isPersonalSelector(WorkspaceFactory.Create(owner: user, isPersonal: true)).Should().Be(true);
        isPersonalSelector(WorkspaceFactory.Create(isPersonal: true)).Should().Be(false);
        isPersonalSelector(WorkspaceFactory.Create(owner: user)).Should().Be(false);
    }

    private static Expression<Func<WorkspaceAggregate, object?>> OrderSelector(string identifier)
    {
        return WorkspaceOrder.OrderProperties.Single(op => op.Identifier == identifier).PropertySelectors.Single();
    }

    [Fact]
    public async Task HandleGetCollectionWorkspaceQuery_WhenUserIsNull_ShouldReturnUserNotFoundError()
    {
        // Arrange
        var query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery();

        _currentUserProvider
            .GetCurrentUser()
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(query, CancellationToken.None);

        // Assert
        await _currentUserProvider
            .Received(1)
            .GetCurrentUser();
        await _workspaceRepository
            .DidNotReceive()
            .GetListByUser(
                Arg.Any<UserAggregate>(),
                Arg.Any<PaginationInfo?>(),
                Arg.Any<IReadOnlyList<IOrder<WorkspaceAggregate>>>(),
                Arg.Any<IEnumerable<IFilter<WorkspaceAggregate>>>());
        await _workspaceRepository
            .DidNotReceive()
            .GetListByUserCount(
                Arg.Any<UserAggregate>(),
                Arg.Any<IEnumerable<IFilter<WorkspaceAggregate>>>());

        outcome.ValidateError(Errors.User.NotFound);
    }
}
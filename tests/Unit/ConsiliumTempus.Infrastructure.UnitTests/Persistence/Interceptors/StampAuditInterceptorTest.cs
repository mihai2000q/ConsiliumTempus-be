using System.Security.Claims;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.ProjectSprint.Entities;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using ConsiliumTempus.Infrastructure.Persistence.Interceptors;
using ConsiliumTempus.Infrastructure.UnitTests.TestUtils;
using FluentAssertions.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ConsiliumTempus.Infrastructure.UnitTests.Persistence.Interceptors;

public class StampAuditInterceptorTest
{
    #region Setup

    private readonly IHttpContextAccessor _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
    private readonly StampAuditInterceptor _uut;
    private readonly ConsiliumTempusDbContext _dbContext = Utils.Persistence.CreateDbContext();

    public StampAuditInterceptorTest()
    {
        _uut = new StampAuditInterceptor(_httpContextAccessor);
    }

    private void LoginAs(string? sub)
    {
        var claims = sub is null ? [] : new[] { new Claim(JwtRegisteredClaimNames.Sub, sub) };
        _httpContextAccessor.HttpContext.Returns(new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(claims))
        });
    }

    private UserAggregate LoginAsATrackedUser()
    {
        var user = UserFactory.Create();
        _dbContext.Attach(user);
        LoginAs(user.Id.Value.ToString());

        return user;
    }

    // A stage that is already saved, so its changes are updates
    private ProjectStage AttachAStage(UserAggregate? createdBy = null)
    {
        var stage = ProjectStageFactory.Create(createdBy: createdBy);
        _dbContext.Attach(stage);

        return stage;
    }

    // A stage that is not saved yet, in a sprint that is
    private ProjectStage AddAStage()
    {
        var sprint = AttachAStage().Sprint;
        var stage = ProjectStage.Create(Name.Create("New"), CustomOrderPosition.Create(0), sprint);
        _dbContext.Add(stage);

        return stage;
    }

    private async Task Stamp()
    {
        await _uut.SavingChangesAsync(Utils.Persistence.CreateEventData(_dbContext), default);
    }

    #endregion

    [Fact]
    public async Task StampAudit_WhenAnEntityIsAdded_ShouldCreateItsAudit()
    {
        // Arrange
        var user = LoginAsATrackedUser();
        var stage = AddAStage();

        // Act
        await Stamp();

        // Assert
        stage.Audit.CreatedBy.Should().Be(user);
        stage.Audit.UpdatedBy.Should().Be(user);
        stage.Audit.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 5.Seconds());
        stage.Audit.UpdatedDateTime.Should().Be(stage.Audit.CreatedDateTime);
    }

    [Fact]
    public async Task StampAudit_WhenAnAddedEntityHasNoOwnedTypes_ShouldCreateItsAudit()
    {
        // Arrange
        var user = LoginAsATrackedUser();
        var workspace = WorkspaceFactory.Create();
        _dbContext.Attach(workspace);
        var membership = Membership.Create(UserFactory.Create(), workspace, WorkspaceRole.Member);
        _dbContext.Attach(membership.User);
        _dbContext.Add(membership);

        // Act
        await Stamp();

        // Assert
        membership.Audit.CreatedBy.Should().Be(user);
    }

    [Fact]
    public async Task StampAudit_WhenAnAddedEntityAlreadyHasAnAudit_ShouldKeepIt()
    {
        // Arrange
        LoginAsATrackedUser();
        var creator = UserFactory.Create();
        var stage = ProjectStageFactory.Create(createdBy: creator);
        var audit = stage.Audit;
        _dbContext.Add(stage);

        // Act
        await Stamp();

        // Assert
        stage.Audit.Should().BeSameAs(audit);
    }

    [Fact]
    public async Task StampAudit_WhenAnEntityIsUpdated_ShouldUpdateItsAudit()
    {
        // Arrange
        var user = LoginAsATrackedUser();
        var creator = UserFactory.Create();
        var stage = AttachAStage(creator);
        var createdDateTime = stage.Audit.CreatedDateTime;
        stage.UpdateCustomOrderPosition(CustomOrderPosition.Create(0));
        stage.Update(Name.Create("Updated"));

        // Act
        await Stamp();

        // Assert
        stage.Audit.CreatedBy.Should().Be(creator);
        stage.Audit.CreatedDateTime.Should().Be(createdDateTime);
        stage.Audit.UpdatedBy.Should().Be(user);
        stage.Audit.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 5.Seconds());
    }

    [Fact]
    public async Task StampAudit_WhenOnlyAChangeExcludedFromTheAuditIsMade_ShouldNotUpdateTheAudit()
    {
        // Arrange
        LoginAsATrackedUser();
        var stage = AttachAStage();
        var audit = stage.Audit;
        stage.UpdateCustomOrderPosition(CustomOrderPosition.Create(10));

        // Act
        await Stamp();

        // Assert
        stage.Audit.Should().BeSameAs(audit);
    }

    [Fact]
    public async Task StampAudit_WhenOnlyAPropertyExcludedFromTheAuditIsChanged_ShouldNotUpdateTheAudit()
    {
        // Arrange
        LoginAsATrackedUser();
        var project = ProjectFactory.Create();
        _dbContext.Attach(project);
        var audit = project.Audit;
        project.RefreshActivity();

        // Act
        await Stamp();

        // Assert
        project.Audit.Should().BeSameAs(audit);
    }

    [Fact]
    public async Task StampAudit_WhenAnEntityIsMarkedAsUpdated_ShouldUpdateItsAuditEvenIfItDidNotChange()
    {
        // Arrange
        var user = LoginAsATrackedUser();
        var stage = AttachAStage();
        _dbContext.MarkAsUpdated(stage);

        // Act
        await Stamp();

        // Assert
        stage.Audit.UpdatedBy.Should().Be(user);
        stage.Audit.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 5.Seconds());
    }

    [Fact]
    public async Task StampAudit_WhenTheEntityMarkedAsUpdatedIsSaved_ShouldForgetIt()
    {
        // Arrange
        LoginAsATrackedUser();
        var stage = AttachAStage();
        _dbContext.MarkAsUpdated(stage);
        await Stamp();
        var audit = stage.Audit;

        // Act
        await Stamp();

        // Assert
        stage.Audit.Should().BeSameAs(audit);
    }

    [Fact]
    public async Task StampAudit_WhenNothingIsChanged_ShouldNotUpdateAnyAudit()
    {
        // Arrange
        LoginAsATrackedUser();
        var stage = AttachAStage();
        var audit = stage.Audit;

        // Act
        await Stamp();

        // Assert
        stage.Audit.Should().BeSameAs(audit);
    }

    [Fact]
    public async Task StampAudit_WhenTheCurrentUserIsBeingDeleted_ShouldStampWithoutAUser()
    {
        // Arrange
        var user = LoginAsATrackedUser();
        var stage = AddAStage();
        _dbContext.Entry(user).State = EntityState.Deleted;

        // Act
        await Stamp();

        // Assert
        stage.Audit.CreatedBy.Should().BeNull();
        stage.Audit.UpdatedBy.Should().BeNull();
    }

    [Fact]
    public async Task StampAudit_WhenTheCurrentUserIdIsNotValid_ShouldStampWithoutAUser()
    {
        // Arrange
        LoginAs("not a guid");
        var stage = AddAStage();

        // Act
        await Stamp();

        // Assert
        stage.Audit.CreatedBy.Should().BeNull();
        stage.Audit.UpdatedBy.Should().BeNull();
    }

    [Fact]
    public async Task StampAudit_WhenNobodyIsLoggedInAndOneUserIsAdded_ShouldStampWithThatUser()
    {
        // Arrange
        LoginAs(null);
        var registeringUser = UserFactory.Create();
        var stage = AddAStage();
        _dbContext.Add(registeringUser);

        // Act
        await Stamp();

        // Assert
        stage.Audit.CreatedBy.Should().Be(registeringUser);
        stage.Audit.UpdatedBy.Should().Be(registeringUser);
    }

    [Fact]
    public async Task StampAudit_WhenThereIsNoHttpContextAndOneUserIsAdded_ShouldStampWithThatUser()
    {
        // Arrange
        _httpContextAccessor.HttpContext.Returns((HttpContext?)null);
        var registeringUser = UserFactory.Create();
        var stage = AddAStage();
        _dbContext.Add(registeringUser);

        // Act
        await Stamp();

        // Assert
        stage.Audit.CreatedBy.Should().Be(registeringUser);
    }

    [Fact]
    public async Task StampAudit_WhenNobodyIsLoggedInAndManyUsersAreAdded_ShouldStampWithoutAUser()
    {
        // Arrange
        LoginAs(null);
        var stage = AddAStage();
        _dbContext.Add(UserFactory.Create());
        _dbContext.Add(UserFactory.Create());

        // Act
        await Stamp();

        // Assert
        stage.Audit.CreatedBy.Should().BeNull();
        stage.Audit.UpdatedBy.Should().BeNull();
    }

    [Fact]
    public async Task StampAudit_WhenNobodyIsLoggedInAndNoUserIsAdded_ShouldStampWithoutAUser()
    {
        // Arrange
        LoginAs(null);
        var stage = AddAStage();

        // Act
        await Stamp();

        // Assert
        stage.Audit.CreatedBy.Should().BeNull();
        stage.Audit.UpdatedBy.Should().BeNull();
    }

    [Fact]
    public async Task StampAudit_WhenThereIsNoDbContext_ShouldDoNothing()
    {
        // Act
        var outcome = await _uut.SavingChangesAsync(Utils.Persistence.CreateEventData(null), default);

        // Assert
        outcome.HasResult.Should().BeFalse();
    }

    [Fact]
    public void StampAudit_WhenSavingSynchronously_ShouldCreateTheAuditOfAnAddedEntity()
    {
        // Arrange
        var user = LoginAsATrackedUser();
        var stage = AddAStage();

        // Act
        _uut.SavingChanges(Utils.Persistence.CreateEventData(_dbContext), default);

        // Assert
        stage.Audit.CreatedBy.Should().Be(user);
    }
}

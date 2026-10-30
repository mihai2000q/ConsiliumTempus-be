using System.Security.Claims;
using ConsiliumTempus.Common.UnitTests.Common.ValueObjects;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.ProjectSprint.Entities;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using ConsiliumTempus.Infrastructure.Persistence.Interceptors;
using ConsiliumTempus.Infrastructure.UnitTests.TestUtils;
using FluentAssertions.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ConsiliumTempus.Infrastructure.UnitTests.Persistence.Interceptors;

public class StampInterceptorTest
{
    #region Setup

    private readonly IHttpContextAccessor _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
    private readonly StampInterceptor _uut;
    private readonly ConsiliumTempusDbContext _dbContext = Utils.Persistence.CreateDbContext();

    public StampInterceptorTest()
    {
        _uut = new StampInterceptor(_httpContextAccessor);
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

    // A task that is already saved, and so is the comment that it has (if there is an author)
    private ProjectTaskAggregate AttachATask(UserAggregate? commentedBy = null, DateTime? commentedAt = null)
    {
        var task = ProjectTaskFactory.Create();
        if (commentedBy is not null)
        {
            var comment = CreateAComment(task);
            comment.WithAuthorship(commentedBy, commentedAt);
            task.AddComment(comment);
        }

        _dbContext.Attach(task);

        return task;
    }

    private static ProjectTaskComment CreateAComment(ProjectTaskAggregate task)
    {
        return ProjectTaskComment.Create(Message.Create("A comment"), task);
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
    public async Task Stamp_WhenAnEntityIsAdded_ShouldCreateItsAudit()
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
    public async Task Stamp_WhenAnAddedEntityHasNoOwnedTypes_ShouldCreateItsAudit()
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
    public async Task Stamp_WhenAnAuthoredEntityIsAdded_ShouldCreateItsAuthorship()
    {
        // Arrange
        var user = LoginAsATrackedUser();
        var task = AttachATask();
        var comment = CreateAComment(task);
        task.AddComment(comment);

        // Act
        await Stamp();

        // Assert
        comment.Authorship.Author.Should().Be(user);
        comment.Authorship.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 5.Seconds());
        comment.Authorship.UpdatedDateTime.Should().Be(comment.Authorship.CreatedDateTime);
    }

    [Fact]
    public async Task Stamp_WhenAnAuthoredEntityIsAddedAndNobodyIsLoggedIn_ShouldCreateItsAuthorshipWithoutAnAuthor()
    {
        // Arrange
        LoginAs(null);
        var task = AttachATask();
        var comment = CreateAComment(task);
        task.AddComment(comment);

        // Act
        await Stamp();

        // Assert
        comment.Authorship.Author.Should().BeNull();
        comment.Authorship.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 5.Seconds());
    }

    [Fact]
    public async Task Stamp_WhenAnAddedAuthoredEntityAlreadyHasAnAuthorship_ShouldKeepIt()
    {
        // Arrange
        LoginAsATrackedUser();
        var task = AttachATask();
        var comment = CreateAComment(task);
        comment.WithAuthorship(UserFactory.Create());
        var authorship = comment.Authorship;
        task.AddComment(comment);

        // Act
        await Stamp();

        // Assert
        comment.Authorship.Should().BeSameAs(authorship);
    }

    [Fact]
    public async Task Stamp_WhenAnAuthoredEntityIsUpdated_ShouldOnlyUpdateTheDateOfItsAuthorship()
    {
        // Arrange
        var author = UserFactory.Create();
        LoginAs(null);
        var createdDateTime = DateTime.UtcNow.AddDays(-1);
        var task = AttachATask(author, createdDateTime);
        var comment = task.Comments.Single();
        typeof(ProjectTaskComment)
            .GetProperty(nameof(ProjectTaskComment.Message))!
            .SetValue(comment, Message.Create("Edited"));

        // Act
        await Stamp();

        // Assert
        comment.Authorship.Author.Should().Be(author);
        comment.Authorship.CreatedDateTime.Should().Be(createdDateTime);
        comment.Authorship.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 5.Seconds());
    }

    [Fact]
    public async Task Stamp_WhenTheAuthorOfAnAuthoredEntityIsCleared_ShouldNotUpdateItsAuthorship()
    {
        // Arrange
        LoginAs(null);
        var author = UserFactory.Create();
        var createdDateTime = DateTime.UtcNow.AddDays(-1);
        var task = AttachATask(author, createdDateTime);
        var comment = task.Comments.Single();
        _dbContext.Entry(comment).Reference(nameof(IAuthored.Authorship)).CurrentValue =
            comment.Authorship.ClearUser(author);

        // Act
        await Stamp();

        // Assert
        comment.Authorship.Author.Should().BeNull();
        comment.Authorship.UpdatedDateTime.Should().Be(createdDateTime);
    }

    [Fact]
    public async Task Stamp_WhenAnAuthoredEntityIsNotChanged_ShouldNotUpdateItsAuthorship()
    {
        // Arrange
        LoginAsATrackedUser();
        var task = AttachATask(UserFactory.Create());
        var authorship = task.Comments.Single().Authorship;

        // Act
        await Stamp();

        // Assert
        task.Comments.Single().Authorship.Should().BeSameAs(authorship);
    }

    [Fact]
    public async Task Stamp_WhenACommentIsAddedToATask_ShouldNotUpdateTheAuditOfTheTask()
    {
        // Arrange
        LoginAsATrackedUser();
        var task = AttachATask();
        var audit = task.Audit;
        task.AddComment(CreateAComment(task));

        // Act
        await Stamp();

        // Assert
        task.Audit.Should().BeSameAs(audit);
    }

    [Fact]
    public async Task Stamp_WhenAnAddedEntityAlreadyHasAnAudit_ShouldKeepIt()
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
    public async Task Stamp_WhenAnEntityIsUpdated_ShouldUpdateItsAudit()
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
    public async Task Stamp_WhenOnlyAChangeExcludedFromTheAuditIsMade_ShouldNotUpdateTheAudit()
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
    public async Task Stamp_WhenOnlyAPropertyExcludedFromTheAuditIsChanged_ShouldNotUpdateTheAudit()
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
    public async Task Stamp_WhenAnEntityIsMarkedAsUpdated_ShouldUpdateItsAuditEvenIfItDidNotChange()
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
    public async Task Stamp_WhenTheEntityMarkedAsUpdatedIsSaved_ShouldForgetIt()
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
    public async Task Stamp_WhenNothingIsChanged_ShouldNotUpdateAnyAudit()
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
    public async Task Stamp_WhenTheCurrentUserIsBeingDeleted_ShouldStampWithoutAUser()
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
    public async Task Stamp_WhenTheCurrentUserIdIsNotValid_ShouldStampWithoutAUser()
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
    public async Task Stamp_WhenNobodyIsLoggedInAndOneUserIsAdded_ShouldStampWithThatUser()
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
    public async Task Stamp_WhenThereIsNoHttpContextAndOneUserIsAdded_ShouldStampWithThatUser()
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
    public async Task Stamp_WhenNobodyIsLoggedInAndManyUsersAreAdded_ShouldStampWithoutAUser()
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
    public async Task Stamp_WhenNobodyIsLoggedInAndNoUserIsAdded_ShouldStampWithoutAUser()
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
    public async Task Stamp_WhenThereIsNoDbContext_ShouldDoNothing()
    {
        // Act
        var outcome = await _uut.SavingChangesAsync(Utils.Persistence.CreateEventData(null), default);

        // Assert
        outcome.HasResult.Should().BeFalse();
    }

    [Fact]
    public void Stamp_WhenSavingSynchronously_ShouldCreateTheAuditOfAnAddedEntity()
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

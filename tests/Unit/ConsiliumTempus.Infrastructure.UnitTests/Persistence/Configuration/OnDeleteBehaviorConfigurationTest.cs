using ConsiliumTempus.Infrastructure.UnitTests.TestUtils;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.UnitTests.Persistence.Configuration;

/// <summary>
/// The relationships are restrictive by default (see <c>RestrictDeleteBehaviorConvention</c>),
/// so the delete of a principal only cascades, or sets to null, what is listed here.
/// A relationship has to be added to the list on purpose, together with its explicit configuration.
/// </summary>
public class OnDeleteBehaviorConfigurationTest
{
    #region Setup

    private static readonly string[] ExpectedCascades =
    [
        "CustomField.ProjectTaskId -> ProjectTaskAggregate",
        "CustomFieldMultiSelectHasOption.MultiSelectCustomFieldId -> MultiSelectCustomField",
        "DateCustomField.SetupId -> DateCustomFieldSetupAggregate",
        "DateCustomFieldSetupAggregate.Id -> CustomFieldSetupAggregate",
        "DateTimeCustomField.SetupId -> DateTimeCustomFieldSetupAggregate",
        "DateTimeCustomFieldSetupAggregate.Id -> CustomFieldSetupAggregate",
        "DurationCustomField.SetupId -> DurationCustomFieldSetupAggregate",
        "DurationCustomFieldSetupAggregate.Id -> CustomFieldSetupAggregate",
        "Membership.UserId -> UserAggregate",
        "Membership.WorkspaceId -> WorkspaceAggregate",
        "MultiSelectCustomField.SetupId -> MultiSelectCustomFieldSetupAggregate",
        "MultiSelectCustomFieldSetupAggregate.Id -> CustomFieldSetupAggregate",
        "MultiSelectOption.SetupId -> MultiSelectCustomFieldSetupAggregate",
        "NumberCustomField.SetupId -> NumberCustomFieldSetupAggregate",
        "NumberCustomFieldSetupAggregate.Id -> CustomFieldSetupAggregate",
        "PeopleCustomField.SetupId -> PeopleCustomFieldSetupAggregate",
        "PeopleCustomFieldSetupAggregate.Id -> CustomFieldSetupAggregate",
        "ProjectAggregate.WorkspaceId -> WorkspaceAggregate",
        "ProjectHasAllowedMember.AllowedMembersId -> UserAggregate",
        "ProjectHasAllowedMember.ProjectAggregateId -> ProjectAggregate",
        "ProjectHasCustomFieldSetup.CustomFieldSetupsId -> CustomFieldSetupAggregate",
        "ProjectHasCustomFieldSetup.ProjectsId -> ProjectAggregate",
        "ProjectSprintAggregate.ProjectId -> ProjectAggregate",
        "ProjectStage.SprintId -> ProjectSprintAggregate",
        "ProjectStatus.ProjectId -> ProjectAggregate",
        "ProjectTaskAggregate.StageId -> ProjectStage",
        "RefreshToken.UserId -> UserAggregate",
        "SingleSelectCustomField.SetupId -> SingleSelectCustomFieldSetupAggregate",
        "SingleSelectCustomFieldSetupAggregate.Id -> CustomFieldSetupAggregate",
        "SingleSelectOption.SetupId -> SingleSelectCustomFieldSetupAggregate",
        "TextCustomField.SetupId -> TextCustomFieldSetupAggregate",
        "TextCustomFieldSetupAggregate.Id -> CustomFieldSetupAggregate",
        "TimeCustomField.SetupId -> TimeCustomFieldSetupAggregate",
        "TimeCustomFieldSetupAggregate.Id -> CustomFieldSetupAggregate",
        "UserHasFavoriteProject.FavoritesId -> UserAggregate",
        "UserHasFavoriteProject.ProjectAggregateId -> ProjectAggregate",
        "UserHasFavoriteWorkspace.FavoritesId -> UserAggregate",
        "UserHasFavoriteWorkspace.WorkspaceAggregateId -> WorkspaceAggregate",
        "WorkspaceInvitation.WorkspaceId -> WorkspaceAggregate",
        "WorkspaceRoleHasPermission.PermissionId -> Permission",
        "WorkspaceRoleHasPermission.WorkspaceRoleId -> WorkspaceRole"
    ];

    private static readonly string[] ExpectedSetNulls =
    [
        "PeopleCustomField.PersonId -> UserAggregate"
    ];

    private static List<(string Name, DeleteBehavior DeleteBehavior)> GetRelationships()
    {
        using var dbContext = Utils.Persistence.CreateDbContext();

        return dbContext.Model
            .GetEntityTypes()
            .SelectMany(e => e.GetDeclaredForeignKeys())
            .Where(fk => !fk.IsOwnership) // the owned types are deleted with their owner
            .Select(fk => (
                Name: $"{fk.DeclaringEntityType.ShortName()}.{string.Join(",", fk.Properties.Select(p => p.Name))}" +
                      $" -> {fk.PrincipalEntityType.ShortName()}",
                fk.DeleteBehavior))
            .ToList();
    }

    #endregion

    [Fact]
    public void OnDeleteBehavior_WhenDeletingAPrincipal_ShouldOnlyCascadeTheListedRelationships()
    {
        // Act
        var cascades = GetRelationships()
            .Where(r => r.DeleteBehavior == DeleteBehavior.Cascade)
            .Select(r => r.Name);

        // Assert
        cascades.Should().BeEquivalentTo(ExpectedCascades);
    }

    [Fact]
    public void OnDeleteBehavior_WhenDeletingAPrincipal_ShouldOnlySetToNullTheListedRelationships()
    {
        // Act
        var setNulls = GetRelationships()
            .Where(r => r.DeleteBehavior == DeleteBehavior.SetNull)
            .Select(r => r.Name);

        // Assert
        setNulls.Should().BeEquivalentTo(ExpectedSetNulls);
    }

    [Fact]
    public void OnDeleteBehavior_WhenDeletingAPrincipal_ShouldNotUseTheClientBehaviorsOrTheDefaultsThatCascadeOrSetToNull()
    {
        // Act
        var otherBehaviors = GetRelationships()
            .Where(r => r.DeleteBehavior is not (DeleteBehavior.Cascade or DeleteBehavior.SetNull
                or DeleteBehavior.Restrict));

        // Assert
        otherBehaviors.Should().BeEmpty();
    }
}

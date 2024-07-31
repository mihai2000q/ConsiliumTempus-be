using ConsiliumTempus.Application.Project.Commands.AddAllowedMember;
using ConsiliumTempus.Application.Project.Commands.AddStatus;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Application.Project.Commands.RemoveAllowedMember;
using ConsiliumTempus.Application.Project.Commands.RemoveStatus;
using ConsiliumTempus.Application.Project.Commands.Update;
using ConsiliumTempus.Application.Project.Commands.UpdateFavorites;
using ConsiliumTempus.Application.Project.Commands.UpdateOverview;
using ConsiliumTempus.Application.Project.Commands.UpdateOwner;
using ConsiliumTempus.Application.Project.Commands.UpdatePrivacy;
using ConsiliumTempus.Application.Project.Commands.UpdateStatus;
using ConsiliumTempus.Application.Project.Queries.Get;
using ConsiliumTempus.Application.Project.Queries.GetAllowedMembers;
using ConsiliumTempus.Application.Project.Queries.GetCollection;
using ConsiliumTempus.Application.Project.Queries.GetStatuses;
using ConsiliumTempus.Common.UnitTests.Project.Entities;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Common.UnitTests.Project;

public static class ProjectResultFactory
{
    public static GetProjectResult CreateGetProjectResult(
        ProjectAggregate? project = null,
        UserAggregate? user = null)
    {
        return new GetProjectResult(
            project ?? ProjectFactory.Create(),
            user ?? UserFactory.Create());
    }
    
    public static GetCollectionProjectResult CreateGetCollectionProjectResult(
        List<ProjectAggregate>? projects = null,
        int totalCount = 25,
        UserAggregate? user = null)
    {
        return new GetCollectionProjectResult(
            projects ?? ProjectFactory.CreateList(),
            totalCount,
            user ?? UserFactory.Create());
    }

    public static GetStatusesFromProjectResult CreateGetStatusesFromProjectResult(
        List<ProjectStatus>? statuses = null,
        int totalCount = 25)
    {
        return new GetStatusesFromProjectResult(
            statuses ?? ProjectStatusFactory.CreateList(),
            totalCount);
    }

    public static GetAllowedMembersFromProjectResult CreateGetAllowedMembersFromProjectResult(
        List<UserAggregate>? allowedMembers = null)
    {
        return new GetAllowedMembersFromProjectResult(
            allowedMembers?.AsReadOnly() ?? UserFactory.CreateList().AsReadOnly());
    }

    public static CreateProjectResult CreateCreateProjectResult()
    {
        return new CreateProjectResult();
    }

    public static AddAllowedMemberToProjectResult CreateAddAllowedMemberToProjectResult()
    {
        return new AddAllowedMemberToProjectResult();
    }

    public static AddStatusToProjectResult CreateAddStatusToProjectResult()
    {
        return new AddStatusToProjectResult();
    }

    public static UpdateProjectResult CreateUpdateProjectResult()
    {
        return new UpdateProjectResult();
    }

    public static UpdateFavoritesProjectResult CreateUpdateFavoritesProjectResult()
    {
        return new UpdateFavoritesProjectResult();
    }

    public static UpdateOverviewProjectResult CreateUpdateOverviewProjectResult()
    {
        return new UpdateOverviewProjectResult();
    }

    public static UpdateOwnerProjectResult CreateUpdateOwnerProjectResult()
    {
        return new UpdateOwnerProjectResult();
    }

    public static UpdatePrivacyProjectResult CreateUpdatePrivacyProjectResult()
    {
        return new UpdatePrivacyProjectResult();
    }

    public static UpdateStatusFromProjectResult CreateUpdateStatusFromProjectResult()
    {
        return new UpdateStatusFromProjectResult();
    }

    public static DeleteProjectResult CreateDeleteProjectResult()
    {
        return new DeleteProjectResult();
    }

    public static RemoveAllowedMemberFromProjectResult CreateRemoveAllowedMemberFromProjectResult()
    {
        return new RemoveAllowedMemberFromProjectResult();
    }

    public static RemoveStatusFromProjectResult CreateRemoveStatusFromProjectResult()
    {
        return new RemoveStatusFromProjectResult();
    }
}
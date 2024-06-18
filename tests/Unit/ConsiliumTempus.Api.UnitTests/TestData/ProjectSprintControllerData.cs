using ConsiliumTempus.Api.Contracts.ProjectSprint.Create;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;

namespace ConsiliumTempus.Api.UnitTests.TestData;

internal static class ProjectSprintControllerData
{
    internal class GetCreateProjectSprintRequests : TheoryData<CreateProjectSprintRequest>
    {
        public GetCreateProjectSprintRequests()
        {
            var request = ProjectSprintRequestFactory.CreateCreateProjectSprintRequest();
            Add(request);
            
            request = ProjectSprintRequestFactory.CreateCreateProjectSprintRequest(
                projectStatus: ProjectSprintRequestFactory.CreateCreateProjectStatus());
            Add(request);
        }
    }
}
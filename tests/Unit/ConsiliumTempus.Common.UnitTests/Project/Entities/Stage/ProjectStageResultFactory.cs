using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Create;
using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Delete;
using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Update;

namespace ConsiliumTempus.Common.UnitTests.Project.Entities.Stage;

public static class ProjectStageResultFactory
{
    public static CreateProjectStageResult CreateCreateProjectStageResult()
    {
        return new CreateProjectStageResult();
    }
    
    public static UpdateProjectStageResult CreateUpdateProjectStageResult()
    {
        return new UpdateProjectStageResult();
    }
    
    public static DeleteProjectStageResult CreateDeleteProjectStageResult()
    {
        return new DeleteProjectStageResult();
    }
}
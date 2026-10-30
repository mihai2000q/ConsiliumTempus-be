-- Every task has a custom field for each setup that its project uses (see 22.project-has-custom-field-setup.sql), like when a setup is added to a project.
-- Ids: bKKNNNNN, where KK is the number of the setup and NNNNN is the number of the task. The values are derived from the number of the task (N),
-- so a part of them is empty on purpose.
DECLARE @Now datetime2 = SYSUTCDATETIME();
DECLARE @Today date = CAST(@Now AS date);

WITH TaskSetup AS (
    SELECT t.Id AS TaskId,
           phs.CustomFieldSetupId AS SetupId,
           CAST(SUBSTRING(CAST(t.Id AS char(36)), 2, 7) AS int) AS N,
           CAST(SUBSTRING(CAST(phs.CustomFieldSetupId AS char(36)), 7, 2) AS int) AS K,
           CONVERT(uniqueidentifier,
               'b' + SUBSTRING(CAST(phs.CustomFieldSetupId AS char(36)), 7, 2) + SUBSTRING(CAST(t.Id AS char(36)), 4, 5)
               + '-0000-0000-0000-000000000000') AS CfId
    FROM ProjectTask t
    JOIN ProjectStage stage ON stage.Id = t.StageId
    JOIN ProjectSprint sprint ON sprint.Id = stage.SprintId
    JOIN ProjectHasCustomFieldSetup phs ON phs.ProjectId = sprint.ProjectId
    JOIN CustomFieldSetup_Date s ON s.Id = phs.CustomFieldSetupId
)
INSERT INTO CustomField_Date (Id, ProjectTaskId, Date, SetupId)
SELECT ts.CfId, ts.TaskId,
       CASE WHEN ts.N % 3 = 0 THEN NULL ELSE DATEADD(day, ts.N % 20 - 10, @Today) END,
       ts.SetupId
FROM TaskSetup ts;

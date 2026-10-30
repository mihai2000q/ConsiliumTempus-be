-- Every task has a custom field for each setup that its project uses (see 22.project-has-custom-field-setup.sql), like when a setup is added to a project.
-- Ids: bKKNNNNN, where KK is the number of the setup and NNNNN is the number of the task. The values are derived from the number of the task (N),
-- so a part of them is empty on purpose.

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
    JOIN CustomFieldSetup_Text s ON s.Id = phs.CustomFieldSetupId
)
INSERT INTO CustomField_Text (Id, ProjectTaskId, Text, SetupId)
SELECT ts.CfId, ts.TaskId,
       CASE WHEN ts.N % 2 = 0 THEN NULL ELSE 'Note for task ' + CAST(ts.N AS varchar(10)) END,
       ts.SetupId
FROM TaskSetup ts;

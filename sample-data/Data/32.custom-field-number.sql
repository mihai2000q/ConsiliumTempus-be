-- Every task has a custom field for each setup that its project uses (see 22.project-has-custom-field-setup.sql), like when a setup is added to a project.
-- Ids: bKKNNNNN, where KK is the number of the setup and NNNNN is the number of the task. The values are derived from the number of the task (N),
-- so a part of them is empty on purpose.
-- K = 3 is Story Points, K = 4 is Budget and K = 11 is Transfer Fee

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
    JOIN CustomFieldSetup_Number s ON s.Id = phs.CustomFieldSetupId
)
INSERT INTO CustomField_Number (Id, ProjectTaskId, Number, SetupId)
SELECT ts.CfId, ts.TaskId,
       CASE WHEN ts.N % 6 = 0 THEN NULL
            WHEN ts.K = 3 THEN CAST(CHOOSE(ts.N % 5 + 1, 1, 2, 3, 5, 8) AS decimal(38, 9))
            WHEN ts.K = 4 THEN CAST(ts.N * 125.50 AS decimal(38, 9))
            ELSE CAST(ts.N * 1000000.00 AS decimal(38, 9)) END,
       ts.SetupId
FROM TaskSetup ts;

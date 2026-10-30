-- The values of the custom fields: every task has a custom field for each setup that its project uses (see ProjectHasCustomFieldSetup),
-- like when a setup is added to a project. The values are derived from the number of the task, so a part of them is empty on purpose.
-- Ids: bKKNNNNN, where KK is the number of the setup and NNNNN is the number of the task.
DECLARE @Now datetime2 = SYSUTCDATETIME();
DECLARE @Today date = CAST(@Now AS date);

-- N is the number of the task and K is the number of the setup
SELECT
    t.Id AS TaskId,
    phs.CustomFieldSetupId AS SetupId,
    CAST(SUBSTRING(CAST(t.Id AS char(36)), 2, 7) AS int) AS N,
    CAST(SUBSTRING(CAST(phs.CustomFieldSetupId AS char(36)), 7, 2) AS int) AS K,
    CONVERT(uniqueidentifier,
        'b' + SUBSTRING(CAST(phs.CustomFieldSetupId AS char(36)), 7, 2) + SUBSTRING(CAST(t.Id AS char(36)), 4, 5)
        + '-0000-0000-0000-000000000000') AS CfId
INTO #TaskSetup
FROM ProjectTask t
JOIN ProjectStage stage ON stage.Id = t.StageId
JOIN ProjectSprint sprint ON sprint.Id = stage.SprintId
JOIN ProjectHasCustomFieldSetup phs ON phs.ProjectId = sprint.ProjectId;

INSERT INTO CustomField_Date (Id, ProjectTaskId, Date, SetupId)
SELECT ts.CfId, ts.TaskId,
       CASE WHEN ts.N % 3 = 0 THEN NULL ELSE DATEADD(day, ts.N % 20 - 10, @Today) END,
       ts.SetupId
FROM #TaskSetup ts
JOIN CustomFieldSetup_Date s ON s.Id = ts.SetupId;

INSERT INTO CustomField_DateTime (Id, ProjectTaskId, DateTime, SetupId)
SELECT ts.CfId, ts.TaskId,
       CASE WHEN ts.N % 3 = 1 THEN NULL ELSE DATEADD(hour, ts.N % 48 - 24, @Now) END,
       ts.SetupId
FROM #TaskSetup ts
JOIN CustomFieldSetup_DateTime s ON s.Id = ts.SetupId;

-- 1 hour = 36 000 000 000 ticks
INSERT INTO CustomField_Duration (Id, ProjectTaskId, Duration, SetupId)
SELECT ts.CfId, ts.TaskId,
       CASE WHEN ts.N % 4 = 0 THEN NULL ELSE CAST(ts.N % 6 + 1 AS bigint) * 36000000000 END,
       ts.SetupId
FROM #TaskSetup ts
JOIN CustomFieldSetup_Duration s ON s.Id = ts.SetupId;

-- K = 3 is Story Points, K = 4 is Budget and K = 11 is Transfer Fee
INSERT INTO CustomField_Number (Id, ProjectTaskId, Number, SetupId)
SELECT ts.CfId, ts.TaskId,
       CASE WHEN ts.N % 6 = 0 THEN NULL
            WHEN ts.K = 3 THEN CAST(CHOOSE(ts.N % 5 + 1, 1, 2, 3, 5, 8) AS decimal(38, 9))
            WHEN ts.K = 4 THEN CAST(ts.N * 125.50 AS decimal(38, 9))
            ELSE CAST(ts.N * 1000000.00 AS decimal(38, 9)) END,
       ts.SetupId
FROM #TaskSetup ts
JOIN CustomFieldSetup_Number s ON s.Id = ts.SetupId;

INSERT INTO CustomField_People (Id, ProjectTaskId, PersonId, SetupId)
SELECT ts.CfId, ts.TaskId,
       CASE WHEN ts.N % 3 = 0 THEN NULL
            ELSE CONVERT(uniqueidentifier, '1000000' + CAST(CHOOSE(ts.N % 4 + 1, 1, 4, 5, 6) AS char(1)) + '-0000-0000-0000-000000000000') END,
       ts.SetupId
FROM #TaskSetup ts
JOIN CustomFieldSetup_People s ON s.Id = ts.SetupId;

-- The options of the setup are used one after the other, by the position
INSERT INTO CustomField_SingleSelect (Id, ProjectTaskId, OptionId, SetupId)
SELECT ts.CfId, ts.TaskId,
       CASE WHEN ts.N % 5 = 0 THEN NULL ELSE opt.Id END,
       ts.SetupId
FROM #TaskSetup ts
JOIN CustomFieldSetup_SingleSelect s ON s.Id = ts.SetupId
OUTER APPLY (
    SELECT o.Id
    FROM SingleSelectOption o
    WHERE o.SetupId = ts.SetupId AND o.CustomOrderPosition = ts.N % 4
) opt;

INSERT INTO CustomField_Text (Id, ProjectTaskId, Text, SetupId)
SELECT ts.CfId, ts.TaskId,
       CASE WHEN ts.N % 2 = 0 THEN NULL ELSE 'Note for task ' + CAST(ts.N AS varchar(10)) END,
       ts.SetupId
FROM #TaskSetup ts
JOIN CustomFieldSetup_Text s ON s.Id = ts.SetupId;

INSERT INTO CustomField_Time (Id, ProjectTaskId, Time, SetupId)
SELECT ts.CfId, ts.TaskId,
       CASE WHEN ts.N % 3 = 2 THEN NULL ELSE CAST(DATEADD(minute, 540 + (ts.N % 6) * 30, CAST('00:00:00' AS time)) AS time) END,
       ts.SetupId
FROM #TaskSetup ts
JOIN CustomFieldSetup_Time s ON s.Id = ts.SetupId;

INSERT INTO CustomField_MultiSelect (Id, ProjectTaskId, SetupId)
SELECT ts.CfId, ts.TaskId, ts.SetupId
FROM #TaskSetup ts
JOIN CustomFieldSetup_MultiSelect s ON s.Id = ts.SetupId;

-- One or two labels per task, and no label for every 5th task
INSERT INTO CustomField_MultiSelectHasOption (CustomFieldId, OptionId)
SELECT ts.CfId, o.Id
FROM #TaskSetup ts
JOIN CustomFieldSetup_MultiSelect s ON s.Id = ts.SetupId
JOIN MultiSelectOption o ON o.SetupId = ts.SetupId
WHERE ts.N % 5 <> 0
  AND (ts.N + o.CustomOrderPosition) % 3 = 0;

DROP TABLE #TaskSetup;

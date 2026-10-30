-- 500 tasks for the Bulk Load Test project (sprint 07), to test pagination, filtering and ordering.
-- Ids: 60001001 - 60001500. Everything is derived from the number of the task, so the data is always the same:
--   stage: 30% Completed, 20% In Review, 20% In Progress, 30% To Do
--   assignee: Michael, Steph, LeBron, Kevin or nobody;  reviewer: nobody or one of Michael, Steph, Kevin
--   due date: between 30 days ago and 29 days ahead, every 7th task has none;  estimate: every 4th task has none
DECLARE @Now datetime2 = SYSUTCDATETIME();
DECLARE @Today date = CAST(@Now AS date);

WITH Numbers AS (
    SELECT TOP (500) CAST(ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS int) AS I
    FROM sys.all_objects a
    CROSS JOIN sys.all_objects b
),
Planned AS (
    SELECT I,
           CASE WHEN I % 10 < 3 THEN 3 WHEN I % 10 < 5 THEN 2 WHEN I % 10 < 7 THEN 1 ELSE 0 END AS StagePosition
    FROM Numbers
),
Ordered AS (
    SELECT I, StagePosition,
           ROW_NUMBER() OVER (PARTITION BY StagePosition ORDER BY I) - 1 AS OrderPosition
    FROM Planned
)
INSERT INTO ProjectTask
    (Id, Name, Description, CustomOrderPosition, IsCompleted, CompletedOn, AssigneeId, ReviewerId, DueDate, EstimatedDuration,
     StageId, CreatedById, CreatedDateTime, UpdatedById, UpdatedDateTime)
SELECT
    CONVERT(uniqueidentifier, '6' + RIGHT('0000000' + CAST(1000 + o.I AS varchar(7)), 7) + '-0000-0000-0000-000000000000'),
    CHOOSE(o.I % 6 + 1, 'Write', 'Review', 'Refactor', 'Deploy', 'Test', 'Document') + ' module ' + CAST(o.I AS varchar(10)),
    CASE WHEN o.I % 3 = 0 THEN '' ELSE 'Generated task number ' + CAST(o.I AS varchar(10)) + ' of the bulk project' END,
    o.OrderPosition,
    CASE WHEN o.StagePosition = 3 THEN 1 ELSE 0 END,
    CASE WHEN o.StagePosition = 3 THEN DATEADD(day, -(o.I % 30), @Now) END,
    CONVERT(uniqueidentifier, '1000000' + CAST(CHOOSE(o.I % 5 + 1, 1, 4, 5, 6, NULL) AS char(1)) + '-0000-0000-0000-000000000000'),
    CONVERT(uniqueidentifier, '1000000' + CAST(CHOOSE(o.I % 4 + 1, NULL, 1, 4, 6) AS char(1)) + '-0000-0000-0000-000000000000'),
    CASE WHEN o.I % 7 = 0 THEN NULL ELSE DATEADD(day, o.I % 60 - 30, @Today) END,
    CASE WHEN o.I % 4 = 0 THEN NULL ELSE DATEADD(minute, (o.I % 8 + 1) * 30, CAST('00:00:00' AS time)) END,
    CONVERT(uniqueidentifier, '5007000' + CAST(o.StagePosition AS char(1)) + '-0000-0000-0000-000000000000'),
    '10000001-0000-0000-0000-000000000000',
    DATEADD(day, -(o.I % 50) - 5, @Now),
    '10000001-0000-0000-0000-000000000000',
    DATEADD(day, -(o.I % 5), @Now)
FROM Ordered o;

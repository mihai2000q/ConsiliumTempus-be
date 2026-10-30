-- Ids: 600000NN, one row per task of the VALUES below (the bulk project adds 60001001-60001500 in 14.bulk-project.sql).
-- Assignee and Reviewer are the N of the user (1000000N), DueDays is relative to today and Estimate is in minutes.
--
-- Covered cases: completed, overdue, due today, without assignee, without reviewer, without due date, without estimate,
-- long name, unicode text, empty stages and tasks in all the stages.
--
--  1-5   Season Planning, Sprint 1 (past, one task was not finished)
--  6-15  Season Planning, Sprint 2 (current)
--  16-18 and 21 Season Planning, Sprint 3 (future)
--  19-20 Season Planning, Backlog
--  22-25 Private Training Plan
--  26-29 Playoffs Retrospective (everything completed, the other stages are empty)
--  30-36 Transfer Window
--  37-39 Personal Goals
DECLARE @Now datetime2 = SYSUTCDATETIME();
DECLARE @Today date = CAST(@Now AS date);

INSERT INTO ProjectTask
    (Id, Name, Description, CustomOrderPosition, IsCompleted, CompletedOn, AssigneeId, ReviewerId, DueDate, EstimatedDuration,
     StageId, CreatedById, CreatedDateTime, UpdatedById, UpdatedDateTime)
SELECT
    CONVERT(uniqueidentifier, '6' + RIGHT('0000000' + CAST(t.No AS varchar(7)), 7) + '-0000-0000-0000-000000000000'),
    t.Name,
    t.Description,
    t.OrderPosition,
    t.Done,
    CASE WHEN t.Done = 1 THEN DATEADD(day, COALESCE(t.DueDays, -2), @Now) END,
    CONVERT(uniqueidentifier, '1000000' + CAST(t.Assignee AS char(1)) + '-0000-0000-0000-000000000000'),
    CONVERT(uniqueidentifier, '1000000' + CAST(t.Reviewer AS char(1)) + '-0000-0000-0000-000000000000'),
    DATEADD(day, t.DueDays, @Today),
    DATEADD(minute, t.Estimate, CAST('00:00:00' AS time)),
    stage.Id,
    project.OwnerId,
    DATEADD(day, -30, @Now),
    project.OwnerId,
    DATEADD(day, -1, @Now)
FROM (VALUES
    -- Season Planning, Sprint 1
    (1,  1, 2, 0, 'Define season goals', 'Agree on the win target and the playoff seeding goal', 1, 1, 4, -20, 120),
    (2,  1, 2, 1, 'Book training facility', '', 1, 4, NULL, -19, 30),
    (3,  1, 2, 2, 'Publish preseason schedule', 'Share the schedule with all the players and the staff', 1, 1, 5, -16, 60),
    (4,  1, 2, 3, 'Order new team jerseys', 'The first batch had the wrong sizes', 1, 6, 1, -15, NULL),
    (5,  1, 1, 0, 'Review last season statistics', 'Carried over from the previous sprint, still waiting for the data', 0, 5, 1, -10, 240),
    -- Season Planning, Sprint 2
    (6,  2, 0, 0, 'Plan travel for away games', '', 0, NULL, NULL, NULL, NULL),
    (7,  2, 0, 1, 'Negotiate sponsorship deal', 'The main sponsor contract expires at the end of the season', 0, 1, NULL, 10, 480),
    (8,  2, 0, 2, 'Update medical protocols', '', 0, 6, 5, -2, 90),
    (9,  2, 1, 0, 'Design playbook for the new offense', 'Focus on the pick and roll variations', 0, 4, 1, 5, 600),
    (10, 2, 1, 1, 'Hire a new nutritionist', '', 0, 5, NULL, 3, NULL),
    (11, 2, 1, 2, 'Film study session 1', '', 0, NULL, 4, 0, 120),
    (12, 2, 2, 0, 'Update the roster on the website', 'The photos need a review before publishing', 0, 6, 4, 2, 45),
    (13, 2, 2, 1, 'Approve training budget', '', 0, 1, NULL, 1, NULL),
    (14, 2, 3, 0, 'Set up the locker room schedule', '', 1, 6, 1, -3, 30),
    (15, 2, 3, 1, 'Confirm referees for the preseason game', 'Done by email', 1, NULL, 1, -4, 15),
    -- Season Planning, Sprint 3
    (16, 3, 0, 0, 'Kick-off party planning', 'Include the families and the sponsors', 0, 4, NULL, 14, 180),
    (17, 3, 0, 1, 'Prepare opening day press release', '', 0, 1, 5, 15, 60),
    (18, 3, 0, 2, 'Rookie orientation', '', 0, NULL, NULL, 18, NULL),
    (21, 3, 1, 0, 'Coordinate the full logistics of the preseason tour, including flights, hotels, local transport, visas, medical insurance and press accreditation for all the staff and players',
     N'Includes the Tokyo leg (東京) and the Zürich stop: pick up the équipe passports.', 0, 4, 1, 16, 960),
    -- Season Planning, Backlog
    (19, 4, 0, 0, 'Explore international exhibition games', 'Low priority idea', 0, NULL, NULL, NULL, NULL),
    (20, 4, 1, 0, 'Build a fan engagement app', 'Long term idea', 0, NULL, NULL, NULL, NULL),
    -- Private Training Plan
    (22, 5, 2, 0, 'Baseline fitness test', '', 1, 4, NULL, -5, 60),
    (23, 5, 1, 0, 'Shooting drills - 500 a day', 'Track the makes, not the attempts', 0, 4, 1, 0, 90),
    (24, 5, 0, 0, 'Recovery plan', 'Ice baths, sleep schedule and stretching', 0, 1, NULL, 7, NULL),
    (25, 5, 0, 1, 'Weight room program', '', 0, NULL, NULL, NULL, NULL),
    -- Playoffs Retrospective
    (26, 6, 2, 0, 'Scout the finals opponent again', '', 1, 6, 1, -100, 240),
    (27, 6, 2, 1, 'Collect the player feedback', '', 1, 6, NULL, -98, 120),
    (28, 6, 2, 2, 'Write the retrospective report', 'Shared with the whole team', 1, 6, 1, -97, 300),
    (29, 6, 2, 3, 'Present the conclusions to the board', '', 1, 1, NULL, -95, 60),
    -- Transfer Window
    (30, 8, 0, 0, 'Shortlist striker targets', '', 0, 2, 3, 6, NULL),
    (31, 8, 1, 0, 'Agent meeting in Lisbon', '', 0, 2, NULL, 2, 180),
    (32, 8, 1, 1, 'Medical for the new signing', '', 0, 3, 2, 4, 120),
    (33, 8, 2, 0, 'Sign the loan agreement', '', 1, 2, 3, -6, 60),
    (34, 8, 2, 1, 'Announce the departure of the captain', '', 1, 3, 2, -8, NULL),
    (35, 9, 0, 0, 'Plan the winter window', '', 0, 2, NULL, 40, NULL),
    (36, 9, 0, 1, 'Review the players on loan', '', 0, NULL, 3, 45, NULL),
    -- Personal Goals
    (37, 11, 0, 0, 'Run a half marathon', '', 0, 1, NULL, 60, NULL),
    (38, 11, 1, 0, 'Learn to play golf', '', 0, 1, NULL, NULL, NULL),
    (39, 11, 2, 0, 'Write a memoir outline', '', 1, 1, NULL, -30, 300)
) AS t(No, Sprint, StagePosition, OrderPosition, Name, Description, Done, Assignee, Reviewer, DueDays, Estimate)
JOIN ProjectStage stage
    ON stage.Id = CONVERT(uniqueidentifier, '5' + RIGHT('000' + CAST(t.Sprint AS varchar(3)), 3) + RIGHT('0000' + CAST(t.StagePosition AS varchar(4)), 4) + '-0000-0000-0000-000000000000')
JOIN ProjectSprint sprint ON sprint.Id = stage.SprintId
JOIN Project project ON project.Id = sprint.ProjectId;

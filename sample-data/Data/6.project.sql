-- Ids: 3000000N.
-- Lifecycle: 0 = Archived, 1 = Active, 2 = Upcoming.
--
-- 1 Season Planning       public, the rich one: 4 sprints, all the custom field types, statuses, comments
-- 2 Private Training Plan private, with allowed members
-- 3 Playoffs Retrospective archived, everything completed
-- 4 Draft Scouting        upcoming, without sprints
-- 5 Bulk Load Test        500 tasks, for pagination, filtering and ordering
-- 6 Transfer Window       Football, public
-- 7 Youth Academy         Football, private without allowed members (only the owner sees it)
-- 8 Personal Goals        private, in a personal workspace
INSERT INTO Project
    (Id, Name, Description, IsPrivate, OwnerId, Lifecycle, LastActivity, WorkspaceId, CreatedById, CreatedDateTime, UpdatedById, UpdatedDateTime)
VALUES
    ('30000001-0000-0000-0000-000000000000', 'Season Planning', 'Plan everything needed for the new season: schedule, sponsors, roster and travel',
     0, '10000001-0000-0000-0000-000000000000', 1, DATEADD(hour, -3, SYSUTCDATETIME()), '20000001-0000-0000-0000-000000000000',
     '10000001-0000-0000-0000-000000000000', '2026-07-20 10:00:00', '10000001-0000-0000-0000-000000000000', '2026-10-28 14:00:00'),
    ('30000002-0000-0000-0000-000000000000', 'Private Training Plan', 'Individual training, visible only to the invited members',
     1, '10000001-0000-0000-0000-000000000000', 1, DATEADD(day, -1, SYSUTCDATETIME()), '20000001-0000-0000-0000-000000000000',
     '10000001-0000-0000-0000-000000000000', '2026-09-01 10:00:00', '10000001-0000-0000-0000-000000000000', '2026-10-25 14:00:00'),
    ('30000003-0000-0000-0000-000000000000', 'Playoffs Retrospective', 'What went well and what did not in last year''s playoffs',
     0, '10000006-0000-0000-0000-000000000000', 0, '2026-07-30 16:00:00', '20000001-0000-0000-0000-000000000000',
     '10000006-0000-0000-0000-000000000000', '2026-07-16 10:00:00', '10000006-0000-0000-0000-000000000000', '2026-07-30 16:00:00'),
    ('30000004-0000-0000-0000-000000000000', 'Draft Scouting', '',
     0, '10000004-0000-0000-0000-000000000000', 2, '2026-10-20 09:00:00', '20000001-0000-0000-0000-000000000000',
     '10000004-0000-0000-0000-000000000000', '2026-10-20 09:00:00', '10000004-0000-0000-0000-000000000000', '2026-10-20 09:00:00'),
    ('30000005-0000-0000-0000-000000000000', 'Bulk Load Test', 'A big project, with 500 tasks, to test pagination, filters and ordering',
     0, '10000001-0000-0000-0000-000000000000', 1, DATEADD(day, -2, SYSUTCDATETIME()), '20000001-0000-0000-0000-000000000000',
     '10000001-0000-0000-0000-000000000000', '2026-08-10 10:00:00', '10000001-0000-0000-0000-000000000000', '2026-10-27 10:00:00'),
    ('30000006-0000-0000-0000-000000000000', 'Transfer Window', 'Everything related to the incoming and outgoing transfers',
     0, '10000002-0000-0000-0000-000000000000', 1, DATEADD(day, -1, SYSUTCDATETIME()), '20000002-0000-0000-0000-000000000000',
     '10000002-0000-0000-0000-000000000000', '2026-08-05 10:00:00', '10000002-0000-0000-0000-000000000000', '2026-10-26 09:08:15'),
    ('30000007-0000-0000-0000-000000000000', 'Youth Academy', 'Only the owner can see this project',
     1, '10000003-0000-0000-0000-000000000000', 1, '2026-10-01 12:00:00', '20000002-0000-0000-0000-000000000000',
     '10000003-0000-0000-0000-000000000000', '2026-09-15 12:00:00', '10000003-0000-0000-0000-000000000000', '2026-10-01 12:00:00'),
    ('30000008-0000-0000-0000-000000000000', 'Personal Goals', '',
     1, '10000001-0000-0000-0000-000000000000', 1, DATEADD(day, -5, SYSUTCDATETIME()), '20000011-0000-0000-0000-000000000000',
     '10000001-0000-0000-0000-000000000000', '2026-06-30 10:00:00', '10000001-0000-0000-0000-000000000000', '2026-10-23 08:10:13');

-- 45 more projects in Basketball, without sprints, to test the pagination of the projects. Ids: 30000101 - 30000145.
-- Mostly Active, some Archived and Upcoming. Every 6th one is private and owned by Michael, the others are public (owned by Michael, Steph or Kevin).
WITH Numbers AS (
    SELECT TOP (45) CAST(ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS int) AS I
    FROM sys.all_objects
),
Projects AS (
    SELECT n.I,
           CASE WHEN n.I % 6 = 0 THEN 1 ELSE CHOOSE(n.I % 3 + 1, 1, 4, 6) END AS Owner
    FROM Numbers n
)
INSERT INTO Project
    (Id, Name, Description, IsPrivate, OwnerId, Lifecycle, LastActivity, WorkspaceId, CreatedById, CreatedDateTime, UpdatedById, UpdatedDateTime)
SELECT
    CONVERT(uniqueidentifier, '3' + RIGHT('0000000' + CAST(100 + p.I AS varchar(7)), 7) + '-0000-0000-0000-000000000000'),
    CHOOSE(p.I % 5 + 1, 'Roster', 'Camp', 'Marketing', 'Scouting', 'Facilities') + ' project ' + CAST(p.I AS varchar(3)),
    CASE WHEN p.I % 2 = 0 THEN '' ELSE 'Generated project number ' + CAST(p.I AS varchar(3)) END,
    CASE WHEN p.I % 6 = 0 THEN 1 ELSE 0 END,
    CONVERT(uniqueidentifier, '1000000' + CAST(p.Owner AS char(1)) + '-0000-0000-0000-000000000000'),
    CHOOSE(p.I % 5 + 1, 1, 1, 1, 0, 2),
    DATEADD(hour, -p.I * 5, SYSUTCDATETIME()),
    '20000001-0000-0000-0000-000000000000',
    CONVERT(uniqueidentifier, '1000000' + CAST(p.Owner AS char(1)) + '-0000-0000-0000-000000000000'),
    DATEADD(day, -p.I - 30, SYSUTCDATETIME()),
    CONVERT(uniqueidentifier, '1000000' + CAST(p.Owner AS char(1)) + '-0000-0000-0000-000000000000'),
    DATEADD(hour, -p.I * 5, SYSUTCDATETIME())
FROM Projects p;

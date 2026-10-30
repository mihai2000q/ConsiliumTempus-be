-- Ids: 2000000N for the shared workspaces, 200000NN for the personal ones (the last digit is the N of the owner).
-- Retired Legends has no projects, and Roger has no personal workspace.
INSERT INTO Workspace
    (Id, Name, Description, OwnerId, IsPersonal, LastActivity, CreatedDateTime, UpdatedDateTime)
VALUES
    ('20000001-0000-0000-0000-000000000000', 'Basketball', 'Everything basketball related: the season, the training and the draft',
     '10000001-0000-0000-0000-000000000000', 0, DATEADD(hour, -2, SYSUTCDATETIME()), '2026-07-12 07:10:13', '2026-10-15 21:08:30'),
    ('20000002-0000-0000-0000-000000000000', 'Football', 'Transfers and the youth academy',
     '10000002-0000-0000-0000-000000000000', 0, DATEADD(day, -1, SYSUTCDATETIME()), '2026-07-23 12:10:13', '2026-10-26 09:08:15'),
    ('20000003-0000-0000-0000-000000000000', 'Retired Legends', '',
     '10000001-0000-0000-0000-000000000000', 0, '2026-08-01 10:00:00', '2026-08-01 10:00:00', '2026-08-01 10:00:00'),
    ('20000011-0000-0000-0000-000000000000', 'Michael Group', 'This is Michael''s workspace',
     '10000001-0000-0000-0000-000000000000', 1, DATEADD(day, -5, SYSUTCDATETIME()), '2026-06-23 08:10:13', '2026-10-23 08:10:13'),
    ('20000012-0000-0000-0000-000000000000', 'My Workspace', '',
     '10000002-0000-0000-0000-000000000000', 1, '2026-06-24 09:00:00', '2026-06-24 09:00:00', '2026-06-24 09:00:00'),
    ('20000013-0000-0000-0000-000000000000', 'My Workspace', '',
     '10000003-0000-0000-0000-000000000000', 1, '2026-06-24 09:30:00', '2026-06-24 09:30:00', '2026-06-24 09:30:00'),
    ('20000014-0000-0000-0000-000000000000', 'My Workspace', '',
     '10000004-0000-0000-0000-000000000000', 1, '2026-06-25 09:00:00', '2026-06-25 09:00:00', '2026-06-25 09:00:00'),
    ('20000015-0000-0000-0000-000000000000', 'My Workspace', '',
     '10000005-0000-0000-0000-000000000000', 1, '2026-06-25 11:00:00', '2026-06-25 11:00:00', '2026-06-25 11:00:00'),
    ('20000016-0000-0000-0000-000000000000', 'My Workspace', '',
     '10000006-0000-0000-0000-000000000000', 1, '2026-06-26 23:00:00', '2026-06-26 23:00:00', '2026-06-26 23:00:00'),
    ('20000017-0000-0000-0000-000000000000', 'My Workspace', '',
     '10000007-0000-0000-0000-000000000000', 1, '2026-09-01 10:00:00', '2026-09-01 10:00:00', '2026-09-01 10:00:00');

-- 30 more workspaces, to test the pagination of the workspaces of Michael. Ids: 20000101 - 20000130.
-- Every 3rd one is owned by Michael, the others by Steph or Kevin (see 3.membership.sql for the roles).
WITH Numbers AS (
    SELECT TOP (30) CAST(ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS int) AS I
    FROM sys.all_objects
)
INSERT INTO Workspace
    (Id, Name, Description, OwnerId, IsPersonal, LastActivity, CreatedDateTime, UpdatedDateTime)
SELECT
    CONVERT(uniqueidentifier, '2' + RIGHT('0000000' + CAST(100 + n.I AS varchar(7)), 7) + '-0000-0000-0000-000000000000'),
    CHOOSE(n.I % 6 + 1, 'Marketing', 'Operations', 'Scouting', 'Finance', 'Community', 'Medical') + ' ' + CAST(n.I AS varchar(3)),
    CASE WHEN n.I % 2 = 0 THEN '' ELSE 'Generated workspace number ' + CAST(n.I AS varchar(3)) END,
    CONVERT(uniqueidentifier, '1000000' + CAST(CHOOSE(n.I % 3 + 1, 1, 4, 6) AS char(1)) + '-0000-0000-0000-000000000000'),
    0,
    DATEADD(hour, -n.I * 9, SYSUTCDATETIME()),
    DATEADD(day, -n.I * 3 - 20, SYSUTCDATETIME()),
    DATEADD(hour, -n.I * 9, SYSUTCDATETIME())
FROM Numbers n;

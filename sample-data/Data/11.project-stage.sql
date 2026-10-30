-- Ids: 5SSS000P, where SSS is the number of the sprint and P is the position of the stage (the first stage of sprint 2 is 50020000).
-- Sprint 10 (Youth Academy) has no stages.
INSERT INTO ProjectStage
    (Id, Name, CustomOrderPosition, SprintId, CreatedDateTime, UpdatedDateTime)
SELECT
    CONVERT(uniqueidentifier, '5' + RIGHT('000' + CAST(s.Sprint AS varchar(3)), 3) + RIGHT('0000' + CAST(s.Position AS varchar(4)), 4) + '-0000-0000-0000-000000000000'),
    s.Name,
    s.Position,
    CONVERT(uniqueidentifier, '4' + RIGHT('0000000' + CAST(s.Sprint AS varchar(7)), 7) + '-0000-0000-0000-000000000000'),
    sprint.CreatedDateTime,
    sprint.CreatedDateTime
FROM (VALUES
    (1, 0, 'To Do'), (1, 1, 'In Progress'), (1, 2, 'Completed'),
    (2, 0, 'To Do'), (2, 1, 'In Progress'), (2, 2, 'In Review'), (2, 3, 'Completed'),
    (3, 0, 'To Do'), (3, 1, 'In Progress'), (3, 2, 'Completed'),
    (4, 0, 'Ideas'), (4, 1, 'Someday'),
    (5, 0, 'To Do'), (5, 1, 'In Progress'), (5, 2, 'Completed'),
    (6, 0, 'To Do'), (6, 1, 'In Progress'), (6, 2, 'Completed'),
    (7, 0, 'To Do'), (7, 1, 'In Progress'), (7, 2, 'In Review'), (7, 3, 'Completed'),
    (8, 0, 'To Do'), (8, 1, 'In Progress'), (8, 2, 'Completed'),
    (9, 0, 'To Do'), (9, 1, 'In Progress'), (9, 2, 'Completed'),
    (11, 0, 'To Do'), (11, 1, 'In Progress'), (11, 2, 'Completed')
) AS s(Sprint, Position, Name)
JOIN ProjectSprint sprint
    ON sprint.Id = CONVERT(uniqueidentifier, '4' + RIGHT('0000000' + CAST(s.Sprint AS varchar(7)), 7) + '-0000-0000-0000-000000000000');

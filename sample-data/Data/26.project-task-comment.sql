-- Ids: a000000N. A comment can have a date and the time spent, both, or none. The comment 3 has no author (deleted user).
DECLARE @Now datetime2 = SYSUTCDATETIME();
DECLARE @Today date = CAST(@Now AS date);

INSERT INTO ProjectTaskComment
    (Id, Message, Date, TimeSpent, TaskId, AuthorId, CreatedDateTime, UpdatedDateTime)
VALUES
    ('a0000001-0000-0000-0000-000000000000', 'The goals were agreed with the board.', DATEADD(day, -20, @Today), '01:30:00',
     '60000001-0000-0000-0000-000000000000', '10000001-0000-0000-0000-000000000000', DATEADD(day, -20, @Now), DATEADD(day, -20, @Now)),
    ('a0000002-0000-0000-0000-000000000000', 'Looks good to me.', null, null,
     '60000001-0000-0000-0000-000000000000', '10000004-0000-0000-0000-000000000000', DATEADD(day, -19, @Now), DATEADD(day, -19, @Now)),
    ('a0000003-0000-0000-0000-000000000000', 'Still waiting for the analytics export.', null, null,
     '60000005-0000-0000-0000-000000000000', null, DATEADD(day, -12, @Now), DATEADD(day, -12, @Now)),
    ('a0000004-0000-0000-0000-000000000000', 'Added the video links for the 3 variations.', DATEADD(day, -1, @Today), '02:15:00',
     '60000009-0000-0000-0000-000000000000', '10000004-0000-0000-0000-000000000000', DATEADD(day, -1, @Now), DATEADD(day, -1, @Now)),
    ('a0000005-0000-0000-0000-000000000000', 'Please add the defensive counters as well.', null, null,
     '60000009-0000-0000-0000-000000000000', '10000001-0000-0000-0000-000000000000', DATEADD(hour, -20, @Now), DATEADD(hour, -20, @Now)),
    ('a0000006-0000-0000-0000-000000000000', 'Done, the doctor needs to sign it.', DATEADD(day, -2, @Today), '00:45:00',
     '60000008-0000-0000-0000-000000000000', '10000006-0000-0000-0000-000000000000', DATEADD(day, -2, @Now), DATEADD(day, -2, @Now)),
    ('a0000007-0000-0000-0000-000000000000', N'Zürich is confirmed, the Tokyo hotel is still open (東京).', null, '00:30:00',
     '60000021-0000-0000-0000-000000000000', '10000004-0000-0000-0000-000000000000', DATEADD(hour, -5, @Now), DATEADD(hour, -5, @Now)),
    ('a0000008-0000-0000-0000-000000000000', 'The agent wants to meet on Monday morning.', null, null,
     '60000031-0000-0000-0000-000000000000', '10000002-0000-0000-0000-000000000000', DATEADD(day, -1, @Now), DATEADD(day, -1, @Now)),
    ('a0000009-0000-0000-0000-000000000000', 'First session done, 87 makes out of 100.', @Today, '01:30:00',
     '60000023-0000-0000-0000-000000000000', '10000004-0000-0000-0000-000000000000', DATEADD(hour, -3, @Now), DATEADD(hour, -3, @Now));

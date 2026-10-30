-- Sample users, all with the password "Password123".
-- Ids: 1000000N. Michael, Steph, LeBron and Kevin are in the Basketball workspace, Leo and Cristiano in Football.
-- Serena only has an invitation and Roger has no workspace at all (the empty state of a new user).
INSERT INTO [User]
    (Id, Email, Password, FirstName, LastName, Role, DateOfBirth, CreatedDateTime, UpdatedDateTime)
VALUES
    ('10000001-0000-0000-0000-000000000000', 'michael@jordan.com', '$2a$13$q2MPesJ2oXqL7EHnK2XnnujuKkryaO2pNeZE4I7usN/IvsPp6bFle',
     'Michael', 'Jordan', 'Pro Basketball Player', '2000-12-12', '2026-06-23 08:10:13', '2026-06-23 09:10:15'),
    ('10000002-0000-0000-0000-000000000000', 'leo@messi.com', '$2a$13$q2MPesJ2oXqL7EHnK2XnnujuKkryaO2pNeZE4I7usN/IvsPp6bFle',
     'Leo', 'Messi', 'Pro Footballer', '2006-09-25', '2026-06-24 09:00:00', '2026-06-24 10:00:00'),
    ('10000003-0000-0000-0000-000000000000', 'cristiano@ronaldo.com', '$2a$13$q2MPesJ2oXqL7EHnK2XnnujuKkryaO2pNeZE4I7usN/IvsPp6bFle',
     'Cristiano', 'Ronaldo', 'The Best Footballer', '2003-06-12', '2026-06-24 09:30:00', '2026-06-24 10:00:00'),
    ('10000004-0000-0000-0000-000000000000', 'stephen@curry.com', '$2a$13$q2MPesJ2oXqL7EHnK2XnnujuKkryaO2pNeZE4I7usN/IvsPp6bFle',
     'Stephen', 'Curry', 'Just Steph', '2006-09-25', '2026-06-25 09:00:00', '2026-06-25 10:00:00'),
    ('10000005-0000-0000-0000-000000000000', 'lebron@james.com', '$2a$13$q2MPesJ2oXqL7EHnK2XnnujuKkryaO2pNeZE4I7usN/IvsPp6bFle',
     'Lebron', 'James', null, '2003-09-25', '2026-06-25 11:00:00', '2026-06-25 12:00:00'),
    ('10000006-0000-0000-0000-000000000000', 'kevin@durant.com', '$2a$13$q2MPesJ2oXqL7EHnK2XnnujuKkryaO2pNeZE4I7usN/IvsPp6bFle',
     'Kevin', 'Durant', 'Basketballer', '1999-01-04', '2026-06-26 23:00:00', '2026-06-27 10:00:00'),
    ('10000007-0000-0000-0000-000000000000', 'serena@williams.com', '$2a$13$q2MPesJ2oXqL7EHnK2XnnujuKkryaO2pNeZE4I7usN/IvsPp6bFle',
     'Serena', 'Williams', 'Pro Tennis Player', '2001-09-26', '2026-09-01 10:00:00', '2026-09-01 10:00:00'),
    ('10000008-0000-0000-0000-000000000000', 'roger@federer.com', '$2a$13$q2MPesJ2oXqL7EHnK2XnnujuKkryaO2pNeZE4I7usN/IvsPp6bFle',
     'Roger', 'Federer', null, null, '2026-10-20 18:00:00', '2026-10-20 18:00:00');

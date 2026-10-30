-- Private Training Plan is visible to Michael (owner), Steph and Kevin, but not to LeBron, even if he is a workspace member.
-- Youth Academy and Personal Goals have no allowed members.
INSERT INTO ProjectHasAllowedMember
    (ProjectId, UserId)
VALUES
    ('30000002-0000-0000-0000-000000000000', '10000004-0000-0000-0000-000000000000'),
    ('30000002-0000-0000-0000-000000000000', '10000006-0000-0000-0000-000000000000');

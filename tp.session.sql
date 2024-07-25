
-- @block
INSERT INTO "Players" ("Name", "PhoneNumber", "Email") VALUES
('John Doe', '+1234567890', 'john.doe@example.com'),
('Jane Smith', '+1234567891', 'jane.smith@example.com'),
('Molly Hock', '+1234567892', 'moly.hock@example.com'),
('Jackson Lee', '+1234567893', 'jackson.lee@gmail.com'),
('Alice Brown', '+1234567894', 'alice.brown@example.com'),
('David Miller', '+1234567895', 'david.miller@yahoo.com'),
('Emily Garcia', '+1234567896', 'emily.garcia@outlook.com'),
('Charles Johnson', '+1234567897', 'charles.johnson@gmail.com');


-- @block
delete from "Players";

-- @block
INSERT INTO "Tournaments" ("Name", "StartDate", "EndDate") VALUES
('Spring Cup', '2024-05-20', '2024-05-26'),
('Summer Open', '2024-06-15', '2024-06-23');


-- @block
-- Spring Cup, Round 1
INSERT INTO "Matches" ("RoundId", "FirstPlayerId", "SecondPlayerId", "IsComplete", "WinnerId") VALUES
(1, 15, 16, true, 15), 
(1, 17, 18, true, 17)

-- @block
-- Spring Cup, Round 2 (Depends on previous round winners)
-- Assuming winners from Round 1 advance to Round 2
INSERT INTO "Matches" ("RoundId", "FirstPlayerId", "SecondPlayerId", "IsComplete", "WinnerId") VALUES
(2, 19, 20, false, NULL);-- John Doe vs. Molly Hock

-- Summer Open, Round 1
INSERT INTO "Matches" ("RoundId", "FirstPlayerId", "SecondPlayerId", "IsComplete", "WinnerId") VALUES
(2, 21,22, false, NULL), -- Alice Brown vs. David Miller
(1, 19, 20, true, 20); -- Emily Garcia vs. Charles Johnson

--DELETE ALL
-- @block
Delete from "Matches";
Delete from "Rounds";
Delete from "Players";
Delete from "Tournaments";

--Drop all table
-- @block
DROP TABLE "Matches";
DROP TABLE "Rounds";
DROP TABLE "Players";
DROP TABLE "Tournaments";

-- @block
DROP TABLE "__EFMigrationsHistory";
INSERT INTO Users (Name, PasswordHash)
VALUES ('yourusername', CONVERT(VARCHAR(32), HashBytes('MD5', 'youruserpassword'), 2))
CREATE TABLE Users(
	UserId int IDENTITY(1,1) NOT NULL,
	Name varchar(50) NOT NULL,
	PasswordHash varchar(32) NOT NULL
);
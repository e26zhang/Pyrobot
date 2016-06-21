
CREATE TABLE [dbo].[User](
	[Id] INT IDENTITY NOT NULL,
	[FlairUpdateRequiredTime] DATETIMEOFFSET(7) NULL,
	[FlairUpdatedTime] DATETIMEOFFSET(7) NULL,
	[IsBanned] BIT NOT NULL,
	[IsAdmin] BIT NOT NULL,
	[Name] NVARCHAR(21) NOT NULL,

	CONSTRAINT PK_Users PRIMARY KEY ([Id])
)
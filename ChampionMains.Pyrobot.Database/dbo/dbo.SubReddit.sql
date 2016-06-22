
CREATE TABLE [dbo].[SubReddit]
(
	[Id] INT IDENTITY NOT NULL,
	[Name] NVARCHAR(21) NOT NULL UNIQUE,
	[ChampionId] SMALLINT NULL,
	CONSTRAINT PK_SubReddits PRIMARY KEY ([Id]),
	CONSTRAINT FK_Champion FOREIGN KEY([ChampionId]) REFERENCES [dbo].[Champion] ([Id])
)
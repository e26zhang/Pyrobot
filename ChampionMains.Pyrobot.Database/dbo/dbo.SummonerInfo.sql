
CREATE TABLE [dbo].[SummonerInfo]
(
	[Id] INT NOT NULL IDENTITY,
	[Division] TINYINT NOT NULL,
	[Tier] TINYINT NOT NULL,
	[UpdatedTime] DATETIMEOFFSET(7) NULL,

	CONSTRAINT PK_SummonerInfo PRIMARY KEY ([Id]),
	CONSTRAINT FK_SummonerInfo_Summoner FOREIGN KEY ([Id]) REFERENCES [dbo].[Summoner] ([Id])
)

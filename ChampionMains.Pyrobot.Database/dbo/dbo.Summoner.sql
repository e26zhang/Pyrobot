
CREATE TABLE [dbo].[Summoner]
(
	[Id] INT IDENTITY NOT NULL,
	[Name] NVARCHAR(21) NOT NULL,
	[Region] NVARCHAR(5) NOT NULL,
	[SummonerId] BIGINT NOT NULL,
	[UserId] INT NOT NULL,

	CONSTRAINT PK_Summoners PRIMARY KEY ([Id])
 )
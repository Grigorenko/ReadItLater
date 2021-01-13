CREATE TABLE [dbo].[Refs]
(
	[Id] [uniqueidentifier] NOT NULL,
	[FolderId] [uniqueidentifier] NULL,
	[Title] [nvarchar](500) NOT NULL,
	[Url] [nvarchar](500) NOT NULL,
	[Image] [nvarchar](500) NULL,
	[Priority] [int] NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	CONSTRAINT [PK_Refs] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Refs_Folders_FolderId] FOREIGN KEY([FolderId]) REFERENCES [dbo].[Folders] ([Id])
)

CREATE TABLE [dbo].[Folders]
(
	[Id] [uniqueidentifier] NOT NULL,
	[ParentId] [uniqueidentifier] NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Order] [int] NOT NULL,
	CONSTRAINT [PK_Folders] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Folders_Folders_ParentId] FOREIGN KEY([ParentId]) REFERENCES [dbo].[Folders] ([Id])
)


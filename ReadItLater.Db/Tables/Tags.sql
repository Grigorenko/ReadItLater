CREATE TABLE [dbo].[Tags]
(
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	CONSTRAINT [PK_Tags] PRIMARY KEY CLUSTERED ([Id] ASC)
)

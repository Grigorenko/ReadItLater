CREATE TABLE [dbo].[TagRefs]
(
	[TagId] [uniqueidentifier] NOT NULL,
	[RefId] [uniqueidentifier] NOT NULL,
	CONSTRAINT [PK_TagRefs] PRIMARY KEY CLUSTERED ([RefId] ASC, [TagId] ASC),
	CONSTRAINT [FK_TagRefs_Refs_RefId] FOREIGN KEY([RefId]) REFERENCES [dbo].[Refs] ([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_TagRefs_Tags_TagId] FOREIGN KEY([TagId]) REFERENCES [dbo].[Tags] ([Id]) ON DELETE CASCADE
)

CREATE PROCEDURE [CreateRef]
	@ref [RefUdt] READONLY,
	@tags [TagUdt] READONLY
AS
	DECLARE @currentId UNIQUEIDENTIFIER;
	SELECT TOP 1 @currentId = COALESCE([Id], NEWID()) FROM @ref;

	INSERT INTO [Refs] ([Id], [FolderId], [Title], [Url], [Image], [Priority], [Date], [Note])
	SELECT TOP 1 @currentId, [FolderId], [Title], [Url], [Image], [Priority], [Date], [Note] FROM @ref;
	
	DECLARE @defaultGuid UNIQUEIDENTIFIER = 0x0;

	MERGE INTO [Tags] AS T
	USING @tags AS S
	ON T.[Name] = S.[Name]
	WHEN NOT MATCHED BY TARGET THEN
		INSERT ([Id], [Name]) 
		VALUES (CASE WHEN S.[Id] = @defaultGuid OR S.[Id] IS NULL THEN NEWID() ELSE S.[Id] END, S.[Name]);

	MERGE INTO [TagRefs] AS T
	USING @tags AS S
	INNER JOIN [Tags] AS ET ON ET.[Name] = S.[Name]
	ON T.[RefId] = @currentId AND T.[TagId] = ET.[Id]
	WHEN NOT MATCHED BY TARGET THEN
		INSERT ([TagId], [RefId])
		VALUES (ET.[Id], @currentId)
	WHEN NOT MATCHED BY SOURCE AND T.[RefId] = @currentId THEN
		DELETE;
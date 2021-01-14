CREATE PROCEDURE [dbo].[UpdateRef]
	@ref [RefUdt] READONLY,
	@tags [TagUdt] READONLY
AS
	DECLARE @currentId UNIQUEIDENTIFIER;
	SELECT TOP 1 @currentId [Id] FROM @ref;

	UPDATE T SET
		T.[Id] = S.[Id], 
		T.[FolderId] = S.[FolderId], 
		T.[Title] = S.[Title], 
		T.[Url] = S.[Url], 
		T.[Image] = S.[Image], 
		T.[Priority] = S.[Priority], 
		T.[Date] = S.[Date]
	FROM [Refs] AS T
	INNER JOIN @ref AS S ON S.[Id] = T.[Id]

	DECLARE @defaultGuid UNIQUEIDENTIFIER = 0x0;

	MERGE INTO [Tags] AS T
	USING @tags AS S
	ON T.[Name] = S.[Name]
	WHEN NOT MATCHED BY TARGET THEN
		INSERT ([Id], [Name]) 
		VALUES (CASE WHEN S.[Id] = @defaultGuid OR S.[Id] IS NULL THEN NEWID() ELSE S.[Id] END, S.[Name])
	WHEN NOT MATCHED BY SOURCE AND (SELECT COUNT(1) FROM [TagRefs] WHERE [RefId] <> @currentId AND [TagId] = T.[Id]) = 0 THEN
		DELETE;

	MERGE INTO [TagRefs] AS T
	USING @tags AS S
	INNER JOIN [Tags] AS ET ON ET.[Name] = S.[Name]
	ON T.[RefId] = @currentId AND T.[TagId] = ET.[Id]
	WHEN NOT MATCHED BY TARGET THEN
		INSERT ([TagId], [RefId])
		VALUES (ET.[Id], @currentId)
	WHEN NOT MATCHED BY SOURCE AND T.[RefId] = @currentId THEN
		DELETE;
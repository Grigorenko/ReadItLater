CREATE PROCEDURE [dbo].[CreateFolder]
	@userId UNIQUEIDENTIFIER,
	@folder [FolderUdt] READONLY
AS
	DECLARE @currentId UNIQUEIDENTIFIER;
	SELECT TOP 1 @currentId = COALESCE(CASE WHEN [Id] = 0x0 THEN NULL ELSE [Id] END, NEWID()) FROM @folder;

	DECLARE @order INT = 0;
	SELECT @order = ISNULL(MAX([Order]), 0) + 10 
	FROM [Folders] 
	WHERE ((SELECT TOP 1 [ParentId] FROM @folder) IS NULL AND [ParentId] IS NULL)
	OR [ParentId] = (SELECT TOP 1 [ParentId] FROM @folder);

	INSERT INTO [Folders] ([Id], [ParentId], [Name], [Order])
	SELECT TOP 1 @currentId, [ParentId], [Name], @order FROM @folder;

	INSERT INTO [UserFolders] ([UserId], [FolderId])
	VALUES (@userId, @currentId);

CREATE PROCEDURE [dbo].[DeleteFolder]
	@id UNIQUEIDENTIFIER
AS
	UPDATE [Refs] SET [FolderId] = NULL WHERE [FolderId] = @id;

	DELETE [Folders] WHERE [ParentId] = @id;
	DELETE [Folders] WHERE [Id] = @id;

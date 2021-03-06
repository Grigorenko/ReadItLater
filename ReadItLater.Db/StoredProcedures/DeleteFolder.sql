CREATE PROCEDURE [dbo].[DeleteFolder]
	@userId UNIQUEIDENTIFIER,
	@id UNIQUEIDENTIFIER
AS
	IF EXISTS (SELECT * FROM [UserFolders] WHERE [UserId] = @userId AND [FolderId] = @id)
	BEGIN
		UPDATE [Refs] SET [FolderId] = NULL WHERE [FolderId] = @id;

		DELETE FROM [UserFolders] WHERE [FolderId] = @id;
		DELETE FROM [Folders] WHERE [ParentId] = @id;
		DELETE FROM [Folders] WHERE [Id] = @id;
	END;

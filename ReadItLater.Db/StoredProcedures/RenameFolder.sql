CREATE PROCEDURE [dbo].[RenameFolder]
	@userId UNIQUEIDENTIFIER,
	@id UNIQUEIDENTIFIER,
	@name NVARCHAR(200)
AS
	UPDATE [Folders] SET [Name] = @name WHERE [Id] = (
		SELECT TOP 1 [FolderId] FROM [UserFolders] WHERE [FolderId] = @id AND [UserId] = @userId);

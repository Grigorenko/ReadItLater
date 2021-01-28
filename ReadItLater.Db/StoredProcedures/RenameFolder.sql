CREATE PROCEDURE [dbo].[RenameFolder]
	@id UNIQUEIDENTIFIER,
	@name NVARCHAR(200)
AS
	UPDATE [Folders] SET [Name] = @name WHERE [Id] = @id;

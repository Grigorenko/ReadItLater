CREATE PROCEDURE [dbo].[MoveUpFolder]
	@id UNIQUEIDENTIFIER
AS
	DECLARE @currentOrder INT;
	DECLARE @prevId UNIQUEIDENTIFIER;
	DECLARE @prevOrder INT;

	SELECT @currentOrder = [Order] 
	FROM [Folders] 
	WHERE [Id] = @id;

	SELECT TOP 1 @prevId = [Id], @prevOrder = [Order] 
	FROM [Folders] 
	WHERE [Order] < @currentOrder 
	ORDER BY [Order] DESC;

	UPDATE [Folders] SET [Order] = @currentOrder WHERE [Id] = @prevId;
	UPDATE [Folders] SET [Order] = @prevOrder WHERE [Id] = @id;

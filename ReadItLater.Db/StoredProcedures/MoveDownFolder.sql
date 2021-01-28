CREATE PROCEDURE [dbo].[MoveDownFolder]
	@id UNIQUEIDENTIFIER
AS
	DECLARE @currentOrder INT;
	DECLARE @nextId UNIQUEIDENTIFIER;
	DECLARE @nextOrder INT;

	SELECT @currentOrder = [Order] 
	FROM [Folders] 
	WHERE [Id] = @id;

	SELECT TOP 1 @nextId = [Id], @nextOrder = [Order] 
	FROM [Folders] 
	WHERE [Order] > @currentOrder 
	ORDER BY [Order] ASC;

	UPDATE [Folders] SET [Order] = @currentOrder WHERE [Id] = @nextId;
	UPDATE [Folders] SET [Order] = @nextOrder WHERE [Id] = @id;

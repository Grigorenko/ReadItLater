CREATE PROCEDURE [dbo].[UpdateCountOfView]
	@refId UNIQUEIDENTIFIER
AS
	DECLARE @current INT;
	SELECT @current = [CountOfView] FROM [Refs] WHERE [Id] = @refId;

	UPDATE [Refs] SET [CountOfView] = @current + 1 WHERE [Id] = @refId;

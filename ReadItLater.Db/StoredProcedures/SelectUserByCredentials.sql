CREATE PROCEDURE [dbo].[SelectUserByCredentials]
	@username NVARCHAR(100),
	@password NVARCHAR(100)
AS
	SELECT TOP 1 * FROM [Users] WHERE [Name] = @username AND [Password] = @password;

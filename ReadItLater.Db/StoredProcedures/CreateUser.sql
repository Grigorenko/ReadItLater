CREATE PROCEDURE [dbo].[CreateUser]
	@id UNIQUEIDENTIFIER,
	@username NVARCHAR(100),
	@password NVARCHAR(100)
AS
	
	INSERT INTO [Users] ([Id], [Name], [Password])
	VALUES (COALESCE(@id, NEWID()), @username, @password);
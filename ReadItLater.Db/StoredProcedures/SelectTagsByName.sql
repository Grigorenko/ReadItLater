CREATE PROCEDURE [dbo].[SelectTagsByName]
	@name NVARCHAR(50)
AS
	SELECT * FROM [Tags] WHERE [Name] LIKE '%' + @name + '%';

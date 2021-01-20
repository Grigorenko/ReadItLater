CREATE PROCEDURE [dbo].[SearchRefs]
	@folderId UNIQUEIDENTIFIER = NULL,
	@tagId UNIQUEIDENTIFIER = NULL,
	@searchTerm NVARCHAR(200),
	@offset INT,
	@limit INT,
	@sort [SortUdt] READONLY
AS
	SELECT *
	FROM [Refs] AS R
	LEFT JOIN [TagRefs] AS TR ON TR.[RefId] = R.[Id]
	LEFT JOIN [Tags] AS T ON T.[Id] = TR.[TagId]
	WHERE [Title] LIKE '%' + @searchTerm + '%'
	OR [Url] LIKE '%' + @searchTerm + '%'
	OR [Note] LIKE '%' + @searchTerm + '%';

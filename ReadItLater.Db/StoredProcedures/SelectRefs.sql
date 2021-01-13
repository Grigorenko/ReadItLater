CREATE PROCEDURE [dbo].[SelectRefs]
	@folderId UNIQUEIDENTIFIER = NULL,
	@tagId UNIQUEIDENTIFIER = NULL,
	@offset INT = 0,
	@limit INT = 25,
	@orderBy NVARCHAR(100) = NULL,
	@direction NVARCHAR(10) = NULL
AS
	IF OBJECT_ID('tempdb..#Refs') IS NOT NULL 
		DROP TABLE #Refs;

	WITH cteNestedFolders([Id], [Name], [ParentId], [Position]) 
	AS (
		SELECT [Id], [Name], [ParentId], 0 FROM [Folders] WHERE [Id] = @folderId
		UNION ALL
		SELECT F.[Id], F.[Name], F.[ParentId], [Position] + 1 FROM [Folders] AS F
		INNER JOIN cteNestedFolders AS CTE ON CTE.[Id] = F.[ParentId]
	)

	SELECT
		R.[Id],
		R.[FolderId],
		R.[Title],
		R.[Image],
		R.[Url],
		R.[Priority],
		R.[Date]
	INTO #Refs
	FROM [Refs] AS R
	LEFT JOIN [TagRefs] AS TR ON TR.[RefId] = R.[Id]
	WHERE (@folderId IS NULL OR R.[FolderId] IN (SELECT [Id] FROM cteNestedFolders))
	AND (@tagId IS NULL OR TR.[TagId] = @tagId)
	GROUP BY R.[Id], R.[FolderId], R.[Title], R.[Image], R.[Url], R.[Priority], R.[Date]
	ORDER BY 
	CASE WHEN @orderBy IS NOT NULL AND @orderBy = 'Priority' AND (@direction IS NULL OR @direction = 'ASC') THEN [Priority] END ASC,
	CASE WHEN @orderBy IS NOT NULL AND @orderBy = 'Priority' AND @direction IS NOT NULL OR @direction = 'DESC' THEN [Priority] END DESC,
	CASE WHEN @orderBy IS NOT NULL AND @orderBy = 'Date' AND (@direction IS NULL OR @direction = 'ASC') THEN [Date] END ASC,
	CASE WHEN @orderBy IS NOT NULL AND @orderBy = 'Date' AND @direction IS NOT NULL OR @direction = 'DESC' THEN [Date] END DESC,
	[Date] DESC OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;

	SELECT * 
	FROM #Refs AS R
	LEFT JOIN [TagRefs] AS TR ON TR.[RefId] = R.[Id]
	LEFT JOIN [Tags] AS T ON T.[Id] = TR.[TagId];
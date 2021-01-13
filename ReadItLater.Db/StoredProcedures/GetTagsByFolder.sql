CREATE PROCEDURE [GetTagsByFolder]
	@folderId UNIQUEIDENTIFIER,
	@orderBy NVARCHAR(100) = NULL,
	@direction NVARCHAR(10) = NULL
AS
	WITH cteNestedFolders([Id], [Name], [ParentId], [Position]) 
	AS (
		SELECT [Id], [Name], [ParentId], 0 FROM [Folders] WHERE [Id] = @folderId
		UNION ALL
		SELECT F.[Id], F.[Name], F.[ParentId], [Position] + 1 FROM [Folders] AS F
		INNER JOIN cteNestedFolders AS CTE ON CTE.[Id] = F.[ParentId]
	)

	SELECT 
		T.[Id], 
		T.[Name],
		COUNT(R.[Id]) AS RefsCount
	FROM [Tags] AS T
	INNER JOIN [TagRefs] AS TR ON TR.[TagId] = T.[Id]
	INNER JOIN [Refs] AS R ON R.[Id] = TR.[RefId]
	INNER JOIN cteNestedFolders AS CTE ON CTE.[Id] = R.[FolderId]
	GROUP BY T.[Id], T.[Name]
	ORDER BY 
	CASE WHEN @orderBy IS NOT NULL AND @orderBy = 'name' AND @direction = 'asc' THEN T.[Name] END ASC,
	CASE WHEN @orderBy IS NOT NULL AND @orderBy = 'name' AND @direction = 'desc' THEN T.[Name] END DESC,
	CASE WHEN @orderBy IS NOT NULL AND @orderBy = 'RefCount' AND @direction = 'asc' THEN T.[Name] END ASC,
	CASE WHEN @orderBy IS NOT NULL AND @orderBy = 'RefCount' AND @direction = 'desc' THEN T.[Name] END DESC,
	[RefsCount] DESC, [Name] ASC;

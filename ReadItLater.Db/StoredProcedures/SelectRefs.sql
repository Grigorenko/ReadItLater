CREATE PROCEDURE [dbo].[SelectRefs]
	@userId UNIQUEIDENTIFIER,
	@folderId UNIQUEIDENTIFIER = NULL,
	@tagId UNIQUEIDENTIFIER = NULL,
	@offset INT,
	@limit INT,
	@sort [SortUdt] READONLY
AS
	IF OBJECT_ID('tempdb..#Refs') IS NOT NULL 
		DROP TABLE #Refs;

	WITH cteNestedFolders([Id], [Name], [ParentId], [Position]) 
	AS (
		SELECT [Id], [Name], [ParentId], 0 FROM [Folders] WHERE [Id] IN (
			SELECT [FolderId] FROM [UserFolders] WHERE [FolderId] = @folderId AND [UserId] = @userId)
		UNION ALL
		SELECT F.[Id], F.[Name], F.[ParentId], [Position] + 1 FROM [Folders] AS F
		INNER JOIN cteNestedFolders AS CTE ON CTE.[Id] = F.[ParentId]
	)
	
	SELECT
		R.[Id],
		R.[FolderId],
		R.[Title],
		R.[Url],
		R.[Image],
		R.[Priority],
		R.[Date],
		R.[Note]
	INTO #Refs
	FROM [Refs] AS R
	LEFT JOIN [TagRefs] AS TR ON TR.[RefId] = R.[Id]
	INNER JOIN [UserFolders] AS UF ON UF.[FolderId] = R.[FolderId]
	WHERE UF.[UserId] = @userId
	AND (@folderId IS NULL OR R.[FolderId] IN (SELECT [Id] FROM cteNestedFolders))
	AND (@tagId IS NULL OR TR.[TagId] = @tagId)
	GROUP BY R.[Id], R.[FolderId], R.[Title], R.[Image], R.[Url], R.[Priority], R.[Date], R.[Note]
	ORDER BY [Date] DESC OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;
	
	--DECLARE @query NVARCHAR(MAX) = '
	--SELECT * 
	--FROM #Refs
	--ORDER BY ' + dbo.GetOrderingOperationQuery(@sort, 'Date DESC') + '
	--OFFSET ' + CAST(@offset AS NVARCHAR(10)) + ' ROWS FETCH NEXT ' + CAST(@limit AS NVARCHAR(10)) + ' ROWS ONLY;';

	--DECLARE @SortedRefs [RefUdt];
	--INSERT INTO @SortedRefs
	--EXECUTE sp_executesql @query;

	SELECT * FROM #Refs AS R
	LEFT JOIN [TagRefs] AS TR ON TR.[RefId] = R.[Id]
	LEFT JOIN [Tags] AS T ON T.[Id] = TR.[TagId];
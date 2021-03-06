CREATE PROCEDURE [SelectFolderListItems]
	@userId UNIQUEIDENTIFIER
AS
	SELECT 
		F.[Id], 
		F.[ParentId], 
		F.[Name],
		F.[Order],
		COUNT(R.[Id]) AS RefsCount
	FROM [Folders] AS F
	LEFT JOIN [Refs] AS R ON R.[FolderId] = F.[Id]
	INNER JOIN [UserFolders] AS UF ON UF.[FolderId] = F.[Id]
	WHERE UF.[UserId] = @userId
	GROUP BY F.[Id], F.[ParentId], F.[Name], F.[Order];
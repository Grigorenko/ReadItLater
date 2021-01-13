CREATE PROCEDURE [SelectFolderListItems]
AS
	SELECT 
		F.[Id], 
		F.[ParentId], 
		F.[Name],
		F.[Order],
		COUNT(R.[Id]) AS RefsCount
	FROM [Folders] AS F
	LEFT JOIN [Refs] AS R ON R.[FolderId] = F.[Id]
	GROUP BY F.[Id], F.[ParentId], F.[Name], F.[Order];
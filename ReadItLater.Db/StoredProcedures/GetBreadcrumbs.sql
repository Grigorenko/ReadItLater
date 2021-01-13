CREATE PROCEDURE [dbo].[GetBreadcrumbs]
	@id UNIQUEIDENTIFIER
AS
WITH cteFolders([Id], [Name], [ParentId], [Position]) 
AS (
    SELECT [Id], [Name], [ParentId], 0 FROM [Folders] WHERE [Id] = @id
    UNION ALL
    SELECT F.[Id], F.[Name], F.[ParentId], [Position] + 1 FROM [Folders] AS F
    INNER JOIN cteFolders AS CTE ON CTE.[ParentId] = F.[Id]
)

SELECT
    [Id], 
    [Name]
FROM cteFolders
ORDER BY [Position] DESC;

CREATE FUNCTION [GetOrderingOperationQuery] 
(
	@sort [SortUdt] READONLY,
	@deafultOrdering NVARCHAR(100) = NULL
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE @query NVARCHAR(MAX)

	SET @query = ISNULL(STUFF((
		SELECT ', ' + T.[OrderBy] + ' ' +  CASE WHEN (SELECT TOP 1 [Direction] FROM @sort WHERE [OrderBy] = T.[OrderBy] ORDER BY [Position] ASC) = 'DESCENDING' THEN 'DESC' ELSE 'ASC' END
		FROM ( 
			SELECT [OrderBy], MIN([Position]) AS [Position] 
			FROM @sort 
			GROUP BY [OrderBy]
		) AS T
		ORDER BY T.[Position]
		FOR XML PATH('')), 1, 2, ''), COALESCE(@deafultOrdering, '1'));

	RETURN @query;

END

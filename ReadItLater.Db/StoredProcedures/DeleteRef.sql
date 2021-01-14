CREATE PROCEDURE [dbo].[DeleteRef]
	@id UNIQUEIDENTIFIER
AS
	IF OBJECT_ID('tempdb..#tags') IS NOT NULL 
		DROP TABLE #tags;

	SELECT
		[TagId]
	INTO #tags
	FROM [TagRefs]
	WHERE [RefId] = @id;

	DELETE FROM [TagRefs] WHERE [RefId] = @id;

	DELETE FROM [Tags] WHERE [Id] IN (
		SELECT [TagId] FROM #tags WHERE [TagId] NOT IN (
			SELECT [TagId] FROM [TagRefs]));

	DELETE FROM [Refs] WHERE [Id] = @id;
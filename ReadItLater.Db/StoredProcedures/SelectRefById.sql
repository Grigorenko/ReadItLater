CREATE PROCEDURE [dbo].[SelectRefById]
	@id UNIQUEIDENTIFIER
AS
	SELECT R.*, T.*
	FROM [Refs] AS R
	LEFT JOIN [TagRefs] AS TR ON TR.[RefId] = R.[Id]
	LEFT JOIN [Tags] AS T ON T.[Id] = TR.[TagId]
	WHERE R.[Id] = @id;

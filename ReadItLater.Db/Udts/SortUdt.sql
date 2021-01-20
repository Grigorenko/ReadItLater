CREATE TYPE [SortUdt]
AS TABLE
(
	[OrderBy] NVARCHAR(50), 
	[Direction] NVARCHAR(10),
	[Position] INT
)
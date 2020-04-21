CREATE PROCEDURE [dbo].[GetRooms]
AS
BEGIN
	SELECT
		[R].[Id] AS [RoomId]
		,[R].[Name]
		,[R].[Beds]
	FROM
		[Room] AS [R]
	ORDER BY
		[Id]
END
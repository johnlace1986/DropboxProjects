CREATE PROCEDURE [dbo].[GetRoomById]
(
	@RoomId AS INT
)
AS
BEGIN
	SELECT
		[R].[Id] AS [RoomId]
		,[R].[Name]
		,[R].[Beds]
	FROM
		[Room] AS [R]
	WHERE
		[R].[Id] = @RoomId
END
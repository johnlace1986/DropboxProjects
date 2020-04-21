CREATE PROCEDURE [DeleteRoom]
(
	@RoomId AS INT
)
AS
BEGIN
	DELETE FROM
		[Room]
	WHERE
		[Id] = @RoomId
END
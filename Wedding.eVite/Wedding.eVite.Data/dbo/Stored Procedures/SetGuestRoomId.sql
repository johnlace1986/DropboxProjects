CREATE PROCEDURE [dbo].[SetGuestRoomId]
(
	@GuestId AS INT
	,@RoomId AS INT
)
AS
BEGIN
	UPDATE
		[Guest]
	SET
		[RoomId] = @RoomId
	WHERE
		[Id] = @GuestId
END
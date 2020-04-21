CREATE PROCEDURE [DeleteGuest]
(
	@GuestId AS INT
)
AS
BEGIN
	DELETE FROM
		[Guest]
	WHERE
		[Id] = @GuestId
END
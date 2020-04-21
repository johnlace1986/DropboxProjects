CREATE PROCEDURE [SetGuestTableId]
(
	@GuestId AS INT
	,@TableId AS INT
)
AS
BEGIN
	UPDATE
		[Guest]
	SET
		[TableId] = @TableId
	WHERE
		[Id] = @GuestId
END
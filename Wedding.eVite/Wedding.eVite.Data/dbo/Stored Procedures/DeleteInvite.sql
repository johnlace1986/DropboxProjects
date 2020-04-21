CREATE PROCEDURE [dbo].[DeleteInvite]
(
	@InviteId AS INT
)
AS
BEGIN
	DELETE FROM
		[Invite]
	WHERE
		[Id] = @InviteId
END
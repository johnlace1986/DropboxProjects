CREATE PROCEDURE [dbo].[DeleteMessage]
(
	@MessageId AS INT
)
AS
BEGIN
	DELETE FROM
		[Message]
	WHERE
		[Id] = @MessageId
END
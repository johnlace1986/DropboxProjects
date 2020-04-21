CREATE PROCEDURE [dbo].[UpdateMessage]
(
	@MessageId AS INT
	,@InviteId AS INT
	,@Body AS VARCHAR(1000)
	,@DateSent AS DATETIME
	,@Sender AS INT
	,@Read AS BIT
)
AS
BEGIN
	UPDATE
		[Message]
	SET
		[InviteId] = @InviteId
		,[Body] = @Body
		,[DateSent] = @DateSent
		,[Sender] = @Sender
		,[Read] = @Read
	WHERE
		[Id] = @MessageId
END
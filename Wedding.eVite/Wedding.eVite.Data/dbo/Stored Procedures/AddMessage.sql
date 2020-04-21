CREATE PROCEDURE [dbo].[AddMessage]
(
	@InviteId AS INT
	,@Body AS VARCHAR(1000)
	,@DateSent AS DATETIME
	,@Sender AS INT
	,@Read AS BIT
)
AS
BEGIN
	INSERT INTO [Message]
	(
		[InviteId]
		,[Body]
		,[DateSent]
		,[Sender]
		,[Read]
	)
	VALUES
	(
		@InviteId
		,@Body
		,@DateSent
		,@Sender
		,@Read
	)

	DECLARE @MessageId INT
	SET @MessageId = SCOPE_IDENTITY()

	SELECT @MessageId AS [NewMessageId]
END
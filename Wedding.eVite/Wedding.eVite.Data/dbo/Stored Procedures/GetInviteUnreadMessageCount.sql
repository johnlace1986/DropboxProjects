CREATE PROCEDURE [dbo].[GetInviteUnreadMessageCount]
(
	@InviteId AS INT
	,@IsRequesterAdmin AS BIT
)
AS
BEGIN

	DECLARE @UnreadMessageCount INT

	IF @IsRequesterAdmin = 1
		BEGIN
			SELECT
				@UnreadMessageCount = COUNT(1)
			FROM
				[Message] AS [M]
			WHERE
				[M].[InviteId] = @InviteId
			AND
				[M].[Sender] = 0 --Sent from invite
			AND
				[Read] = 0	
		END
	ELSE
		BEGIN
			SELECT
				@UnreadMessageCount = COUNT(1)
			FROM
				[Message] AS [M]
			WHERE
				[M].[InviteId] = @InviteId
			AND
				[M].[Sender] = 1 --Sent from admin
			AND
				[Read] = 0
		END

	SELECT @UnreadMessageCount AS [UnreadMessageCount]
END
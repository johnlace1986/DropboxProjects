CREATE PROCEDURE [dbo].[GetUnreadMessagesByInviteId]
(
	@InviteId AS INT
	,@IsRequesterAdmin AS BIT
)
AS
BEGIN

	CREATE TABLE #MessageIds ([MessageId] INT NOT NULL)

	INSERT INTO
		#MessageIds
	SELECT
		[M].[Id]
	FROM
		[Message] AS [M]
	WHERE
		[M].[InviteId] = @InviteId
	AND
		[M].[Read] = 0
	AND
	(
		(
				@IsRequesterAdmin = 0
			AND
				[M].[Sender] = 1
		)
		OR
		(
				@IsRequesterAdmin = 1
			AND
				[M].[Sender] = 0
		)
	)

	UPDATE
		[Message]
	SET
		[Read] = 1
	WHERE
		[Id] IN
		(
			SELECT
				[MessageId]
			FROM
				#MessageIds
		)

	SELECT 
		[M].[Id] AS [MessageId]
		,[M].[InviteId]
		,[M].[Body]
		,[M].[DateSent]
		,[M].[Sender]
		,[M].[Read]
	FROM
		[Message] AS [M]
	WHERE
		[M].[Id] IN
		(
			SELECT
				[MessageId]
			FROM
				#MessageIds
		)
	ORDER BY
		[M].[DateSent]
END
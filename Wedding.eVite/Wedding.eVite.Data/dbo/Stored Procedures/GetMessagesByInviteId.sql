CREATE PROCEDURE [GetMessagesByInviteId]
(
	@InviteId AS INT
)
AS
BEGIN
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
		[M].[InviteId] = @InviteId
	ORDER BY
		[M].[DateSent]
END
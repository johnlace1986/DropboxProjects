CREATE PROCEDURE [dbo].[GetInvites]
AS
BEGIN
	SELECT
		[I].[Id] AS [InviteId]
		,[I].[EmailAddress]
		,[I].[Password]
		,[I].[HasChangedPassword]
		,[I].[IsAdmin]
		,[I].[IncludesCeremony]
		,[I].[ReserveSandholeRoom]
		,[I].[EmailMessages]
		,[I].[NotifyGiftWebsite]
	FROM
		[Invite] AS [I]
END
CREATE PROCEDURE [dbo].[GetUnassignedTableGuests]
AS
BEGIN
	SELECT
		[G].[Id] AS [GuestId]
		,[G].[InviteId]
		,[G].[Forename]
		,[G].[Surname]
		,[G].[IsAttending]
		,[G].[DateOfRsvp]
		,[G].[IsChild]
		,[G].[IsVegetarian]
		,[G].[CanBringPlusOne]
		,[G].[IsBringingPlusOne]
		,[G].[PlusOneForename]
		,[G].[PlusOneSurname]
		,[G].[PlusOneIsVegetarian]
		,[G].[TableId]
		,[G].[RoomId]
		,[G].[Notes]
	FROM
		[Guest] AS [G]
	INNER JOIN
		[Invite] AS [I] ON [I].[Id] = [G].[InviteId]
	WHERE
		[G].[TableId] IS NULL
	AND
		([G].[IsAttending] IS NULL OR [G].[IsAttending] = 1)
	AND
		[I].[IncludesCeremony] = 1
END
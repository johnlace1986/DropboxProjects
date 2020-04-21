CREATE PROCEDURE [dbo].[GetGuestsByInviteId]
(
	@InviteId AS INT
)
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
	WHERE
		[G].[InviteId] = @InviteId
		
END
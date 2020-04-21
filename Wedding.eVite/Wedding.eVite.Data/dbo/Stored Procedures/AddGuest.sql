CREATE PROCEDURE [dbo].[AddGuest]
(
	@InviteId AS INT
	,@Forename AS VARCHAR(50)
	,@Surname AS VARCHAR(50)
	,@IsAttending AS BIT
	,@DateOfRsvp AS DATETIME
	,@IsChild AS BIT
	,@IsVegetarian AS BIT
	,@CanBringPlusOne AS BIT
	,@IsBringingPlusOne AS BIT
	,@PlusOneForename AS VARCHAR(50)
	,@PlusOneSurname AS VARCHAR(50)
	,@PlusOneIsVegetarian AS BIT
	,@TableId AS INT
	,@RoomId AS INT
	,@Notes AS VARCHAR(500)
)
AS
BEGIN
	DECLARE @GuestId INT

	INSERT INTO [Guest]
	(
		[InviteId]
		,[Forename]
		,[Surname]
		,[IsAttending]
		,[DateOfRsvp]
		,[IsChild]
		,[IsVegetarian]
		,[CanBringPlusOne]
		,[IsBringingPlusOne]
		,[PlusOneForename]
		,[PlusOneSurname]
		,[PlusOneIsVegetarian]
		,[TableId]
		,[RoomId]
		,[Notes]
	)
	VALUES
	(
		@InviteId
		,@Forename
		,@Surname
		,@IsAttending
		,@DateOfRsvp
		,@IsChild
		,@IsVegetarian
		,@CanBringPlusOne
		,@IsBringingPlusOne
		,@PlusOneForename
		,@PlusOneSurname
		,@PlusOneIsVegetarian
		,@TableId
		,@RoomId
		,@Notes
	)

	SET @GuestId = SCOPE_IDENTITY()

	SELECT @GuestId AS [NewGuestId]
END
CREATE PROCEDURE [dbo].[UpdateGuest]
(
	@GuestId AS INT
	,@InviteId AS INT
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
	UPDATE
		[Guest]
	SET
		[InviteId] = @InviteId
		,[Forename] = @Forename
		,[Surname] = @Surname
		,[IsAttending] = @IsAttending
		,[DateOfRsvp] = @DateOfRsvp
		,[IsChild] = @IsChild
		,[IsVegetarian] = @IsVegetarian
		,[CanBringPlusOne] = @CanBringPlusOne
		,[IsBringingPlusOne] = @IsBringingPlusOne
		,[PlusOneForename] = @PlusOneForename
		,[PlusOneSurname] = @PlusOneSurname
		,[PlusOneIsVegetarian] = @PlusOneIsVegetarian
		,[TableId] = @TableId
		,[RoomId] = @RoomId
		,[Notes] = @Notes
	WHERE
		[Id] = @GuestId
END
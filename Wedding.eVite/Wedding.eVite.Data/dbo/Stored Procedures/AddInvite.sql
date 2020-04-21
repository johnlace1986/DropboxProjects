CREATE PROCEDURE [dbo].[AddInvite]
(
	@EmailAddress AS VARCHAR(50)
	,@Password AS VARCHAR(256)
	,@HasChangedPassword AS BIT
	,@IsAdmin AS BIT
	,@IncludesCeremony AS BIT
	,@ReserveSandholeRoom AS BIT
	,@EmailMessages AS BIT
	,@NotifyGiftWebsite AS BIT
)
AS
BEGIN
	INSERT INTO [Invite]
	(
		[EmailAddress]
		,[Password]
		,[HasChangedPassword]
		,[IsAdmin]
		,[IncludesCeremony]
		,[ReserveSandholeRoom]
		,[EmailMessages]
		,[NotifyGiftWebsite]
	)
	VALUES
	(
		@EmailAddress
		,@Password
		,@HasChangedPassword
		,@IsAdmin
		,@IncludesCeremony
		,@ReserveSandholeRoom
		,@EmailMessages
		,@NotifyGiftWebsite
	)

	DECLARE @InviteId INT
	SET @InviteId = SCOPE_IDENTITY()

	SELECT @InviteId AS [NewInviteId]
END
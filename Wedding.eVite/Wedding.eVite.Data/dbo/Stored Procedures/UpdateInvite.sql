CREATE PROCEDURE [dbo].[UpdateInvite]
(
	@InviteId AS INT
	,@EmailAddress AS VARCHAR(50)
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
	UPDATE
		[Invite]
	SET
		[EmailAddress] = @EmailAddress
		,[Password] = @Password
		,[HasChangedPassword] = @HasChangedPassword
		,[IsAdmin] = @IsAdmin
		,[IncludesCeremony] = @IncludesCeremony
		,[ReserveSandholeRoom] = @ReserveSandholeRoom
		,[EmailMessages] = @EmailMessages
		,[NotifyGiftWebsite] = @NotifyGiftWebsite
	WHERE
		[Id] = @InviteId
END
CREATE PROCEDURE [AddRoom]
(
	@Name AS VARCHAR(50)
	,@Beds AS INT
)
AS
BEGIN
	INSERT INTO [Room]
	(
		[Name]
		,[Beds]
	)
	VALUES
	(
		@Name
		,@Beds
	)

	SELECT CONVERT(INT, SCOPE_IDENTITY()) AS [NewRoomId]
END
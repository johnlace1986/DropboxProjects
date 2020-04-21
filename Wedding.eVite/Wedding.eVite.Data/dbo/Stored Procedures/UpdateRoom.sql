CREATE PROCEDURE [UpdateRoom]
(
	@RoomId AS INT
	,@Name AS VARCHAR(50)
	,@Beds AS INT
)
AS
BEGIN
	UPDATE
		[Room]
	SET
		[Name] = @Name
		,[Beds] = @Beds
	WHERE
		[Id] = @RoomId
END
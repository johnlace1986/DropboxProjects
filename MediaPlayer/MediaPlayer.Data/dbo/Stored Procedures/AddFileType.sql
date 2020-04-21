CREATE PROCEDURE [AddFileType]
(
	@Name AS VARCHAR(50),
	@MediaItemType AS SMALLINT
)
AS
BEGIN
	IF  EXISTS (SELECT 1 FROM [FileType] WHERE [Name] = @Name)
	BEGIN
		RAISERROR('Specified name already exists', 11, 1)
		RETURN
	END
	
	INSERT INTO [FileType]
	(
		[Name],
		[MediaItemType]
	)
	VALUES
	(
		@Name,
		@MediaItemType
	)
	
	SELECT CONVERT(SMALLINT, SCOPE_IDENTITY()) AS [NewId]
END
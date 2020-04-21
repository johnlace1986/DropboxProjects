CREATE PROCEDURE [UpdateFileType]
(
	@FileTypeId AS SMALLINT,
	@Name AS VARCHAR(50),
	@MediaItemType AS SMALLINT
)
AS
BEGIN
	IF NOT EXISTS (SELECT 1 FROM [FileType] WHERE [Id] = @FileTypeId)
	BEGIN
		RAISERROR('Specified file type ID does not exist', 11, 1)
		RETURN
	END
	
	IF  EXISTS (SELECT 1 FROM [FileType] WHERE [Id] != @FileTypeId AND [Name] = @Name)
	BEGIN
		RAISERROR('Specified name already exists', 11, 1)
		RETURN
	END
	
	UPDATE
		[FileType]
	SET
		[Name] = @Name,
		[MediaItemType] = @MediaItemType
	WHERE
		[Id] = @FileTypeId
	
	SELECT CONVERT(SMALLINT, SCOPE_IDENTITY()) AS [NewId]
END
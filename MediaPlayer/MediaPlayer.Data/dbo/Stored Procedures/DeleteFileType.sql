CREATE PROCEDURE [DeleteFileType]
(
	@FileTypeId AS SMALLINT
)
AS
BEGIN
	IF NOT EXISTS (SELECT 1 FROM [FileType] WHERE [Id] = @FileTypeId)
	BEGIN
		RAISERROR('Specified file type ID does not exist', 11, 1)
		RETURN
	END
	
	DELETE FROM
		[FileType]
	WHERE
		[Id] = @FileTypeId
	
	SELECT CONVERT(SMALLINT, SCOPE_IDENTITY()) AS [NewId]
END
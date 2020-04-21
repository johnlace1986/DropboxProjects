CREATE PROCEDURE [GetFileTypeById]
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
	
	SELECT
		[Id],
		[Name],
		[MediaItemType]
	FROM
		[FileType]
	WHERE
		[Id] = @FileTypeId
		
END
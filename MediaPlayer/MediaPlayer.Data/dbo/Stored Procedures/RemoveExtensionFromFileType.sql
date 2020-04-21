CREATE PROCEDURE [RemoveExtensionFromFileType]
(
	@FileTypeId AS SMALLINT,
	@Extension AS VARCHAR(50)
)
AS
BEGIN
	IF NOT EXISTS (SELECT 1 FROM [FileType] WHERE [Id] = @FileTypeId)
	BEGIN
		RAISERROR('Specified file type ID does not exist', 11, 1)
		RETURN
	END
	
	IF NOT EXISTS (SELECT 1 FROM [FileTypeExtension] WHERE [FileTypeId] = @FileTypeId AND [Extension] = @Extension)
	BEGIN
		RAISERROR('Extension does not belong to file type', 11, 1)
		RETURN
	END
	
	DELETE FROM
		[FileTypeExtension]
	WHERE
		[FileTypeId] = @FileTypeId
	AND
		[Extension] = @Extension	
END
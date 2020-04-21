CREATE PROCEDURE [AddExtensionToFileType]
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
	
	IF EXISTS (SELECT 1 FROM [FileTypeExtension] WHERE [FileTypeId] = @FileTypeId AND [Extension] = @Extension)
	BEGIN
		RAISERROR('Extension already belongs to file type', 11, 1)
		RETURN
	END
	
	INSERT INTO [FileTypeExtension]
	(
		[FileTypeId],
		[Extension]
	)
	VALUES
	(
		@FileTypeId,
		@Extension
	)		
END
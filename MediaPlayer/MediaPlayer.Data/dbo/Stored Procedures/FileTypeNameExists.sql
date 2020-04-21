CREATE PROCEDURE [dbo].[FileTypeNameExists]
(
	@Name AS VARCHAR(50)
)
AS
BEGIN
	IF EXISTS
	(
		SELECT 1 FROM
			[FileType]
		WHERE
			[Name] = @Name
	)
	BEGIN
		SELECT CONVERT(BIT, 1) AS [FileTypeNameExists]
	END
	ELSE
	BEGIN
		SELECT CONVERT(BIT, 0) AS [FileTypeNameExists]
	END		
END
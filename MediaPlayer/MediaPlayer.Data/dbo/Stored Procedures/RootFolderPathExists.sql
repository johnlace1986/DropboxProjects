CREATE PROCEDURE [dbo].[RootFolderPathExists]
(
	@MediaItemType AS SMALLINT,
	@Path AS VARCHAR(255)
)
AS
BEGIN
	IF EXISTS
	(
		SELECT 1 FROM
			[RootFolder]
		WHERE
			[MediaItemType] = @MediaItemType
		AND
			[Path] = @Path
	)
	BEGIN
		SELECT CONVERT(BIT, 1) AS [RootFolderPathExists]
	END
	ELSE
	BEGIN
		SELECT CONVERT(BIT, 0) AS [RootFolderPathExists]
	END		
END
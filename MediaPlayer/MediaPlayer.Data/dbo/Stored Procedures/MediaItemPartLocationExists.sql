CREATE PROCEDURE [dbo].[MediaItemPartLocationExists]
(
	@Location AS VARCHAR(255)
)
AS
BEGIN
	IF EXISTS
	(
		SELECT 1 FROM
			[MediaItemPart]
		WHERE
			[Location] = @Location
	)
	BEGIN
		SELECT CONVERT(BIT, 1) AS [MediaItemPartLocationExists]
	END
	ELSE
	BEGIN
		SELECT CONVERT(BIT, 0) AS [MediaItemPartLocationExists]
	END		
END
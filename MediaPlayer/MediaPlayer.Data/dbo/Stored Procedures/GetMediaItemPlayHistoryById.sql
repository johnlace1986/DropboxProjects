﻿CREATE PROCEDURE [dbo].[GetMediaItemPlayHistoryById]
(
	@MediaItemId AS BIGINT,
	@MediaItemType AS SMALLINT
)
AS
BEGIN
	IF NOT EXISTS (SELECT 1 FROM [MediaItem] WHERE [Id] = @MediaItemId AND [Type] = @MediaItemType)
	BEGIN
		RAISERROR('Specified media item ID does not exist', 11, 1)
		RETURN
	END
	
	SELECT
		[MediaItemId],
		[MediaItemType],
		[DatePlayed]
	FROM
		[MediaItemPlayHistory]
	WHERE
		[MediaItemId] = @MediaItemId
	AND
		[MediaItemType] = @MediaItemType
	ORDER BY
		[DatePlayed]
END
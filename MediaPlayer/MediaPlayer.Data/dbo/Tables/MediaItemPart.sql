CREATE TABLE [dbo].[MediaItemPart] (
    [Location]      VARCHAR (255) NOT NULL,
    [MediaItemId]   BIGINT        NOT NULL,
    [MediaItemType] SMALLINT      NOT NULL,
    [Size]          BIGINT        NOT NULL,
    [Duration]      INT           NOT NULL,
    [Index]         SMALLINT      NOT NULL,
    CONSTRAINT [PK_MediaItemPart_1] PRIMARY KEY CLUSTERED ([Location] ASC),
    CONSTRAINT [FK_MediaItemPart_MediaItem] FOREIGN KEY ([MediaItemId], [MediaItemType]) REFERENCES [dbo].[MediaItem] ([Id], [Type]) ON DELETE CASCADE ON UPDATE CASCADE
);


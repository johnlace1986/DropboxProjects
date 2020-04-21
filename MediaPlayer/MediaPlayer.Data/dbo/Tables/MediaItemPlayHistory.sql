CREATE TABLE [dbo].[MediaItemPlayHistory] (
    [MediaItemId]   BIGINT   NOT NULL,
    [MediaItemType] SMALLINT NOT NULL,
    [DatePlayed]    DATETIME NOT NULL,
    CONSTRAINT [PK_MediaItemPlayHistory] PRIMARY KEY CLUSTERED ([MediaItemId] ASC, [MediaItemType] ASC, [DatePlayed] ASC),
    CONSTRAINT [FK_MediaItemPlayHistory_MediaItem] FOREIGN KEY ([MediaItemId], [MediaItemType]) REFERENCES [dbo].[MediaItem] ([Id], [Type]) ON DELETE CASCADE ON UPDATE CASCADE
);

